using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataSource;
using Data.Model;

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
