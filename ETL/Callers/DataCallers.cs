// ***********************************************************************
// Assembly         : ETL
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="DataCallers.cs" company="">
//     Copyright ©  2019 Mariusz Owczarski
// </copyright>
// <summary></summary>
// ***********************************************************************
using Contract.Interfaces;
using Contract.Model;
using DataContext.DataSource;
using System.Collections.Generic;

namespace ETL.Callers
{
    /// <summary>
    /// Class DataCallers.
    /// Implements the <see cref="Contract.Interfaces.ICaller" />
    /// </summary>
    /// <seealso cref="Contract.Interfaces.ICaller" />
    public class DataCallers : ICaller
    {
        // JEST TO KLASA KTORA ZAWIERA IMPLEMENTACJE WZORCA SINGLETON
        // ZAPEWANIA ONA INSTANCJE DOSTEPU DO SERWISU W PROJEKCIE DataContext
        // WAZNE BY IMPLEMENTOWALA TEN SAM INTEFEJS CO SERWIS 

        /// <summary>
        /// The instance
        /// </summary>
        static DataCallers instance = null;
        /// <summary>
        /// The movie service
        /// </summary>
        static MovieService movieService = null;
        /// <summary>
        /// The padlock
        /// </summary>
        static readonly object padlock = new object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
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

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get => movieService.ConnectionString; set => movieService.ConnectionString = value; }

        /// <summary>
        /// Edits the movie.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool EditMovie(Movie movie)
        {
            bool result = false;
            result = movieService.EditMovie(movie);
            return result;
        }

        /// <summary>
        /// Adds the movies.
        /// </summary>
        /// <param name="movies">The movies.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AddMovies(List<Movie> movies)
        {
            bool result = false;
            result = movieService.AddMovies(movies);
            return result;
        }

        /// <summary>
        /// Gets all movies.
        /// </summary>
        /// <returns>List&lt;Movie&gt;.</returns>
        public List<Movie> GetAllMovies()
        {
            List<Movie> result = null;
            result = movieService.GetAllMovies();
            return result;
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool RemoveAll()
        {
            bool result = false;
            result = movieService.RemoveAll();
            return result;
        }

        /// <summary>
        /// Determines whether [is server connected].
        /// </summary>
        /// <returns><c>true</c> if [is server connected]; otherwise, <c>false</c>.</returns>
        public bool IsServerConnected()
        {
            bool result = false;
            result = movieService.IsServerConnected();
            return result;
        }
    }
}
