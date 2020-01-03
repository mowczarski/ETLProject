using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Interfaces;
using Contract.Model;
using DataContext.Dtos;

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

        public bool AddMovies(List<Movie> movies)
        {
            using (var context = new ETLModel())
            {
                try
                {
                    var movieDtoList = new List<MovieDto>();

                    foreach (var movie in movies)
                        movieDtoList.Add(movie.Set());

                    context.Movies.AddRange(movieDtoList);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception();
                }
            }
        }

        public List<Movie> GetAllMovies()
        {
            using (var context = new ETLModel())
            {
                try
                {
                    var result = context.Movies.ToList();

                    return result.Select(x => x.Get()).ToList();

                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception();
                }
            }
        }

        public bool RemoveAll()
        {
            using (var context = new ETLModel())
            {
                try
                {
                    var movies = context.Movies.ToList();
                    context.Movies.RemoveRange(movies);
                    context.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception();
                }
            }
        }
    }
}
