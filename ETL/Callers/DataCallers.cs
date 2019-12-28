using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContext.DataSource;
using DataContext.Model;

namespace ETL.Callers
{
    public class DataCallers : ICaller
    {
        MovieService movies = new MovieService();
        public void AddMovie(Movie movie)
        {
            movies.AddMovie(movie);
        }
    }
}
