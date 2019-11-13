using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataSource
{
    public class MovieService
    {
        private ETLModel db = new ETLModel();
        public void AddMovie(Movie movie)
        {
            db.Movies.Add(movie);
            db.SaveChanges();
        }

        public void AddType(MovieType type)
        {
            db.MovieTypes.Add(type);
            db.SaveChanges();
        }
    }
}
