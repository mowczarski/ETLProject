using Contract.Enum;
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
            return DataCallers.Instance.GetAllMovies();
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

            if (dataGrid.Columns.Count > 0)
                dataGrid.Columns[0].Header = "ID";
        }

        private void Scrap(int from, int to)
        {
            FilmWebScraper scraper = new FilmWebScraper();
            scraper.TextBoxValueChanged += OtherWindowOnTextBoxValueChanged;
            scraper.ScrapeMovies(from, to);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            WriteToConsole(DateTime.Now + " - Scrapping started");
            var numberOfOperationPerThread = 36 / proc;

            for (int i = -1 ; i < proc; ++i)
            {
                Thread t = new Thread(() => Scrap((int)(numberOfOperationPerThread * i) + 1, (int)(numberOfOperationPerThread * (i + 1))));
                t.Start();
                WriteToConsole(DateTime.Now + @" - Thread : {i} started");
                Thread.Sleep(500);                
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Setp1_Click(object sender, RoutedEventArgs e)
        {
            WriteToConsole(DateTime.Now + " - Scrapping started");
            var numberOfOperationPerThread = 24 / proc;

            for (int i = -1; i < proc; ++i)
            {
                Thread t = new Thread(() => Scrap((int)(numberOfOperationPerThread * i) + 1, (int)(numberOfOperationPerThread * (i + 1))));
                t.Start();
                WriteToConsole(DateTime.Now + @" - Thread : {i} started");
                Thread.Sleep(500);
            }
        }

        private void Setp2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jsons";

                string[] files = Directory.GetFiles(path, "*.txt")
                                     .Select(Path.GetFileName)
                                     .ToArray();

                foreach(var file in files)
                {
                    var movieList = new List<Movie>();
                    var filePath = path + $@"\{file}";

                    using (StreamReader sr = new StreamReader(filePath))
                    {
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

                            movieList.Add(movie);  
                        }
                        Thread t = new Thread(() => DataCallers.Instance.AddMovies(movieList));
                        t.Start();                 
                    }

                    File.Delete(filePath);
                    Thread t2 = new Thread(() => Search());
                    t2.Start();
                }
            }
            catch (IOException ex)
            {
                WriteToConsole("The file could not be read:");
                WriteToConsole(ex.Message);
            }
        }

        private void Setp3_Click(object sender, RoutedEventArgs e)
        {
            var person = new List<Person>();
            person.Add(new Person
            {
                Name = "Mario",
                Surname = "Kowcek",
            });


            var type = new List<MovieType>();
            type.Add(new MovieType
            {
                Name = "Komedia",
                Type = MOVIE_TYPE.KOMEDIA,
                Description = "Śmieszny film",
            });

            var movie = new Movie
            {
                Title = "Zielona Mila",
                OrginalTitle = "Green Mile",
                Rank = 12,
                Year = 1995,
                Duration = "20 min",
                Director = "Jan Antek",
                Rate = 4,
                RateTotalVotes = 351214,
                Description = "Film o zle skazanym ludziu",
                ReleaseDate = DateTime.Now,
                BoxOffice = 3256478974,
                Production = PRODUCTION_COUNTRY.ANGLIA,
                Staff = person,
                Types = type,
            };

            DataCallers.Instance.AddMovie(movie);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search(searchTB.Text);
        }

        public void WriteToConsole(string txt)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                ConsoleOut.Text += txt + Environment.NewLine;
            })); 
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            DataCallers.Instance.RemoveAll();
            Search();
        }
    }
}