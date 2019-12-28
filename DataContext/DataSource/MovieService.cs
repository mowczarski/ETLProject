using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Interfaces;
using Contract.Model;

namespace DataContext.DataSource
{
    public class MovieService : ICaller
    {
        public bool AddMovie(Movie movie)
        {
            using (var context = new ETLModel())
            {
                try
                {
                    context.Movies.Add(movie.Set());
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new System.NotImplementedException();
                }
            }
        }

        public IList<Movie> GetAllMovies()
        {
            using (var context = new ETLModel())
            {
                try
                {
                    return context.Movies.ToList().Select(x => x.Get()).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception();

                }
            }
        }
    }
}
