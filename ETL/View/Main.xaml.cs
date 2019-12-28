using Contract.Enum;
using Contract.Model;
using ETL.Callers;
using ETL.Helpers;
using ETL.Webscraper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ETL.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        FilmWebScraper scraper = new FilmWebScraper();
        List<MovieModel> list = new List<MovieModel>();
        TextOutputterr outputter;
        int? list2 = null;
        readonly int proc = Environment.ProcessorCount;
        public Main()
        {
          
            InitializeComponent();
            //var movies = caller.GetAllMovies();
            //dataGrid.ItemsSource = movies;
            outputter = new TextOutputterr(ConsoleOut);
            Console.SetOut(outputter);
            Console.WriteLine("Started");

            var timer1 = new Timer(TimerTick, "Timer1", 0, 100);
            var timer2 = new Timer(TimerTick, "Timer2", 0, 50);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Number Of Logical Processors: {0}", Environment.ProcessorCount);

            var numberOfOperationPerThread = 500 / proc;

            //l1.Content += DateTime.Now.ToString(); 
            list2 = (int)DateTime.Now.TimeOfDay.TotalMilliseconds;

            for (int i = -1; i < proc; ++i)
            {
                Thread t = new Thread(() => Scrap((int)(numberOfOperationPerThread * i) + 1 , (int)(numberOfOperationPerThread * (i + 1))));
                t.Start();
            }

        }

        private void Scrap(int from, int to) {

            list.AddRange(scraper.ScrapeMovies(from, to));

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                //l1.Content += "\n" + DateTime.Now.TimeOfDay.ToString() + "\nTotal of " + $"{(int)DateTime.Now.TimeOfDay.TotalMilliseconds - list2 }" + " ms" + "\nCount " + $"{list.Count.ToString()}";
                }));

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        void TimerTick(object state)
        {
            var who = state as string;
            Console.WriteLine(who);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var movies = DataCallers.Instance.GetAllMovies();
            dataGrid.ItemsSource = movies;
            //DataCallers.Instance.AddMovie(new Movie { });
            //caller.AddMovie(new Movie { });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

            var person = new List<Person>();
            person.Add(new Person
            {
                Name = "Mariusz",
                Surname = "Owczarski",
                isActor = true,
                isDirector = false,
                isScenarist = false,
                isPhotographer = false,
                isComposer = false,
                isDescription = false
            });


            var type = new List<MovieType>();
            type.Add( new MovieType
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
                Rate = 4,
                RateTotalVotes = 351214,
                DistributionCompany = "ECDC",
                Description = "Film o zle skazanym ludziu",
                ReleaseDate = DateTime.Now,
                Studio = "Studio Niedziwedz",
                BoxOffice = 3256478974,
                Production = PRODUCTION_COUNTRY.ANGLIA,
                Staff = person,
                Types = type,
            };

            DataCallers.Instance.AddMovie(movie);

            //DataCallers.Instance.AddMovie(new Movie { });
            //caller.AddMovie(new Movie { });
        }
    }
}



//FilmWebScraper scraper;
//DataCallers db = new DataCallers();
//public MainWindow()
//{

//    InitializeComponent();
//    scraper = new FilmWebScraper();

//    DataContext = scraper;
//    var movies = scraper.ScrapeMovies(1, 500);
//    System.Diagnostics.Debug.Print(movies.Count.ToString());
//    foreach (var movie in movies)
//    {

//        //db.AddMovie(new Data.Model.Movie { Title = movie.Title });

//        System.Diagnostics.Debug.Print(movie.Title);
//        System.Diagnostics.Debug.Print(movie.Description);
//        System.Diagnostics.Debug.Print(movie.Director);
//        System.Diagnostics.Debug.Print(movie.Actors.Count.ToString());
//    }
//}