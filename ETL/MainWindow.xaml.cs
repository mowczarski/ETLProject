using ETL.Callers;
using ETL.Webscraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ETL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        FilmWebScraper scraper;
        DataCallers db = new DataCallers();
        public MainWindow()
        {

            InitializeComponent();
            scraper = new FilmWebScraper();

            DataContext = scraper;
            var movies = scraper.ScrapeMovies();
            System.Diagnostics.Debug.Print(movies.Count.ToString());
            foreach(var movie in movies){

                //db.AddMovie(new Data.Model.Movie { Title = movie.Title });

                System.Diagnostics.Debug.Print(movie.Title);
                System.Diagnostics.Debug.Print(movie.Description);
                System.Diagnostics.Debug.Print(movie.Director);
                System.Diagnostics.Debug.Print(movie.Actors.Count.ToString());
            }
        }
    }
}
