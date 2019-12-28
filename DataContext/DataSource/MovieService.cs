using DataContext.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.DataSource
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

        public void AddPerson(Person type)
        {
            db.Persons.Add(type);
            db.SaveChanges();
        }
    }
}
