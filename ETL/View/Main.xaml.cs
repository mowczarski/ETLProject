using Contract.Model;
using ETL.Callers;
using ETL.Helpers;
using ETL.Webscraper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using static ETL.Webscraper.FilmWebScraper;

namespace ETL.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {      
        List<Movie> movies = null;
        List<Movie> moviesToLoad = new List<Movie>();
        readonly int proc = Environment.ProcessorCount;

        public Main()
        {
            InitializeComponent();
            
            Thread t = new Thread(() => Search());
            t.Start();

            WriteToConsole("Application Started");
            WriteToConsole($"Number Of Logical Processors: {Environment.ProcessorCount}");
        }

        private void OtherWindowOnTextBoxValueChanged(object sender, TextBoxValueEventArgs e)
        {
            ConsoleOut.Text += e.NewValue + Environment.NewLine;
        }

        private object loadingLock = new object();
        public void Search(string txt = null)
        {
            WriteToConsole("Search initialized");

            lock (loadingLock)
            {
                try
                {
                       movies = UpdateData();

                    Dispatcher.Invoke((Action)(() =>
                    {
                        UpdateView(txt);

                    }));
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }
            }
        }

        private List<Movie> UpdateData()
        {
            WriteToConsole("UpdateData started");
            var result = DataCallers.Instance.GetAllMovies();
            WriteToConsole("UpdateData finished");
            return result;
        }

        public void UpdateView(string txt)
        {
            dataGrid.ItemsSource = null;
            
            if (String.IsNullOrEmpty(txt))
            {
                dataGrid.ItemsSource = movies;
            }
            else
            {
                dataGrid.ItemsSource = movies.Where(x =>
                    x.OrginalTitle.Contains(txt)
                    || x.Title.Contains(txt)
                    || x.Staff.Any(y => y.Name.Contains(txt)
                    || x.Types.Any(z => z.Name.Contains(txt))));
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Extract();
            Transform();
            Load();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            moviesToLoad = null;
            movies = null;
            dataGrid.ItemsSource = null;
            Step2.IsEnabled = false;
            Step3.IsEnabled = false;
        }

        private void Setp1_Click(object sender, RoutedEventArgs e)
        {
            if (Step2.IsEnabled == true)
            {
                WriteToConsole("Cannot run Extract proces Again");
                return;
            }

            Extract();
            Step2.IsEnabled = true;       
        }

        private void Setp2_Click(object sender, RoutedEventArgs e)
        {
            if (Step3.IsEnabled == true)
            {
                WriteToConsole("Cannot run Transform proces Again");
                return;
            }

            Transform();
            Step3.IsEnabled = true;
        }

        private void Extract()
        {
            WriteToConsole("Scrapping started");
            var numberOfOperationPerThread = 24 / proc;

            for (int i = -1; i < proc; ++i)
            {
                Thread t = new Thread(() => Scrap((int)(numberOfOperationPerThread * i) + 1, (int)(numberOfOperationPerThread * (i + 1))));
                t.Start();
                WriteToConsole(@"Thread : {i} started");
                Thread.Sleep(500);
            }
        }

        private void Scrap(int from, int to)
        {
            FilmWebScraper scraper = new FilmWebScraper();
            scraper.TextBoxValueChanged += OtherWindowOnTextBoxValueChanged;
            scraper.ScrapeMovies(from, to);
        }

        private void Transform()
        {
            try
            {
                WriteToConsole("Transform started");
                WriteToConsole("Find json directory");
                var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jsons";

                string[] files = Directory.GetFiles(path, "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

                foreach (var file in files)
                {
                    var movieList = new List<Movie>();
                    var filePath = path + $@"\{file}";

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        WriteToConsole($"Transfrom {file} started");
                        String line = sr.ReadToEnd();
                        dynamic json = JsonConvert.DeserializeObject(line);

                        foreach (var it in json)
                        {
                            var movie = new Movie
                            {
                                BoxOffice = Converters.ConvertToDecimal(it.BoxOffice),
                                Description = Converters.ConvertToString(it.Description),
                                Director = Converters.ConvertToString(it.Director),
                                Duration = Converters.ConvertToString(it.Duration),
                                ReleaseDate = Converters.ConvertToDateTime(it.ReleaseDate),
                                Rank = Converters.ConvertToInt(it.Rank),
                                Title = Converters.ConvertToString(it.Title),
                                OrginalTitle = Converters.ConvertToString(it.OrginalTitle),
                                Production = Converters.ConvertToProduction(it.Production),
                                Rate = Converters.ConvertToInt(it.Rate),
                                RateTotalVotes = Converters.ConvertToInt(it.RateTotalVotes),
                                Year = Converters.ConvertToInt(it.Year),
                            };

                            movie.Staff = new List<Person>();
                            foreach (var person in it.Staff)
                            {
                                movie.Staff.Add(new Person
                                {
                                    Name = Converters.ConvertToString(person.Name),
                                    Surname = Converters.ConvertToString(person.Surname),
                                });
                            }

                            movie.Types = new List<MovieType>();
                            foreach (var type in it.Types)
                            {
                                movie.Types.Add(new MovieType
                                {
                                    Description = Converters.ConvertToString(type.Description),
                                    Name = Converters.ConvertToString(type.Name),
                                    Type = Converters.ConvertToMovieType(type.Type),

                                });
                            }
                            WriteToConsole($"Transfrom moovie: {movie.Title}, by {movie.Director}");
                            WriteToConsole($"Transfrom {file} finished");
                            movieList.Add(movie);                           
                        }
                        moviesToLoad.AddRange(movieList);
                    }
                    WriteToConsole($"Delete json files: {filePath}");
                    File.Delete(filePath);
                }
            }
            catch (IOException ex)
            {
                WriteToConsole("The file could not be read:");
                WriteToConsole(ex.Message);
            }
        }

        private void Load(List<Movie> movies = null)
        {
            if (movies != null)
            {
                WriteToConsole("Load to data base movies (count) - " + movies.Count);
                Thread t = new Thread(() => DataCallers.Instance.AddMovies(movies));
                t.Start();
                WriteToConsole("Load to data base started");
                t.Join();
                WriteToConsole("Load to data base finished");
            }
            else
            {
                WriteToConsole("Load to data base movies (count) - " + moviesToLoad.Count);
                Thread t = new Thread(() => DataCallers.Instance.AddMovies(moviesToLoad));
                t.Start();
                WriteToConsole("Load to data base started");
                t.Join();
                WriteToConsole("Load to data base finished");
            }
          
            
            Thread t2 = new Thread(() => Search());
            t2.Start();
        }

        private void Setp3_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() => Search());
            t.Start();
        }

        public void WriteToConsole(string txt)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                ConsoleOut.Text += DateTime.Now + " - " + txt + Environment.NewLine;
            })); 
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            WriteToConsole("Delete All Click started");
            DataCallers.Instance.RemoveAll();
            
            WriteToConsole("Delete All finished");
            Thread t = new Thread(() => Search());
            t.Start();
        }
    }
}