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
        List<Movie> dbMovies = null;
        List<Movie> moviesToLoad = new List<Movie>();
        readonly int proc = Environment.ProcessorCount;
        FilmWebScraper scraper = new FilmWebScraper();

        public Main()
        {
            InitializeComponent();
            scraper.TextBoxValueChanged += OtherWindowOnTextBoxValueChanged;
            WriteToConsole("Application Started");
            WriteToConsole($"Number Of Logical Processors: {Environment.ProcessorCount}");
            ConsoleScrollViewer.ScrollToBottom();
            Thread t = new Thread(() => Search());
            t.Start();
        }

        private void OtherWindowOnTextBoxValueChanged(object sender, TextBoxValueEventArgs e)
        {
            WriteToConsole(e.NewValue);
        }

        private object loadingLock = new object();
        public void Search(string txt = null)
        {
            WriteToConsole("Search initialized");

            lock (loadingLock)
            {
                try
                {
                    if (dbMovies == null)
                        dbMovies = UpdateData();

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
            listViewUsers.ItemsSource = null;

            if (String.IsNullOrEmpty(txt))
            {
                listViewUsers.ItemsSource = dbMovies;
            }
            else
            {
                listViewUsers.ItemsSource = dbMovies.Where(x =>
                    (!string.IsNullOrEmpty(x.OrginalTitle) && x.OrginalTitle.Contains(txt))
                    || (!string.IsNullOrEmpty(x.OrginalTitle) && x.Title.Contains(txt))
                    || (x.Staff != null && x.Staff.Any(y => y.Name.Contains(txt)))
                    || (x.Types != null && x.Types.Any(z => z.Name.Contains(txt))));
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
            dbMovies = null;
            listViewUsers.ItemsSource = null;
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
        private object extractLock = new object();
        private void Extract()
        {
            WriteToConsole("Scrapping started");

            for (int i = 0; i <= proc; i++)
            {
                try
                {
                    Thread t = new Thread(() => Scrap(i));
                    t.Start();
                    Thread.Sleep(50);
                    WriteToConsole($"Thread : {i} started");
                }
                catch (Exception ex)
                {
                    WriteToConsole("Error with threads");
                    WriteToConsole(ex.Message);
                }
            }
        }

        private void Scrap(int page)
        {
            scraper.ScrapeMovies(page);
            WriteToConsole($"Thread : {page} finished");
        }

        private void Transform()
        {
            try
            {
                WriteToConsole("Transform started");
                WriteToConsole("Find json directory");
                string path = null;

                try
                {
                    path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jsons";
                }
                catch (Exception ex)
                {
                    WriteToConsole("Error with directory path");
                    WriteToConsole(ex.Message);
                    return;
                }

                string[] files = Directory.GetFiles(path, "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

                if (files == null || files.Length == 0)
                {
                    WriteToConsole("Error with finding files");
                    return;
                }

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

                            WriteToConsole($"Transform finished - movie: {movie.Title}, by {movie.Director}");
                            movieList.Add(movie);
                        }
                        WriteToConsole($"Transfrom of {file} finished");
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
                return;
            }
        }

        private void Load(List<Movie> movies = null)
        {
            if (movies != null)
            {
                WriteToConsole("Load to data base movies (count) - " + movies.Count);
                Thread thread = new Thread(() => DataCallers.Instance.AddMovies(movies));
                thread.Start(); WriteToConsole("Load to database started");
                thread.Join(); WriteToConsole("Load to database finished");
            }
            else
            {
                WriteToConsole("Load to data base movies (count) - " + moviesToLoad.Count);
                Thread thread = new Thread(() => DataCallers.Instance.AddMovies(moviesToLoad));

                thread.Start(); WriteToConsole("Load to data base started");
                thread.Join(); WriteToConsole("Load to data base finished");
            }

            Thread thread2 = new Thread(() => Search());
            thread2.Start();
        }

        private void Setp3_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchParameter = searchTB.Text;
            Thread t = new Thread(() => Search(searchParameter));
            t.Start();
        }

        public void WriteToConsole(string txt)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ConsoleOut.Text += "INFO " + DateTime.Now + "                - " + txt + Environment.NewLine;
            }));
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            WriteToConsole("Delete All started");
            DataCallers.Instance.RemoveAll();

            WriteToConsole("Delete All finished");
            Thread t = new Thread(() => Search());
            t.Start();
        }
    }
}