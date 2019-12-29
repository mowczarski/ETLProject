﻿using Contract.Interfaces;
using Contract.Model;
using DataContext.DataSource;
using System.Collections.Generic;

namespace ETL.Callers
{
    public class DataCallers : ICaller
    {
        static DataCallers instance = null;
        static MovieService movieService = null;
        static readonly object padlock = new object();

        public static DataCallers Instance
        {
            get
            {
                if (instance == null || movieService == null)
                {
                    lock (padlock)
                    {
                        movieService = new MovieService();
                        instance = new DataCallers();
                    }
                }
                return instance;
            }
        }

        public bool AddMovie(Movie movie)
        {
            bool result = false;
            result = movieService.AddMovie(movie);
            return result;
        }

        public bool AddMovies(List<Movie> movies)
        {
            bool result = false;
            result = movieService.AddMovies(movies);
            return result;
        }

        public List<Movie> GetAllMovies()
        {
            List<Movie> result = null;
            result = movieService.GetAllMovies();
            return result;
        }

        public bool RemoveAll()
        {
            bool result = false;
            result = movieService.RemoveAll();
            return result;
        }
    }
}
