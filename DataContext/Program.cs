using DataContext.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContext.Model;

namespace DataContext
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.ReadKey();
            MovieService db = new MovieService();
            db.AddMovie(new Model.Movie { });
            db.AddType(new Model.MovieType { Description = "elo", Name = "Komedia", Type = 0 });
        }
    }
}
