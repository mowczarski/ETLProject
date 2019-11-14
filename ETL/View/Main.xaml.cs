using ETL.Webscraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        int? list2 = null;
        readonly int proc = Environment.ProcessorCount;
        public Main()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Number Of Logical Processors: {0}", Environment.ProcessorCount);

            var numberOfOperationPerThread = 500 / proc;

            l1.Content += DateTime.Now.ToString(); 
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
                l1.Content += "\n" + DateTime.Now.TimeOfDay.ToString() + "\nTotal of " + $"{(int)DateTime.Now.TimeOfDay.TotalMilliseconds - list2 }" + " ms" + "\nCount " + $"{list.Count.ToString()}";
                }));

        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
