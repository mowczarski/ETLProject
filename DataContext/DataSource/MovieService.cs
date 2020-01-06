// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="MovieService.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Interfaces;
using Contract.Model;
using DataContext.Dtos;

namespace DataContext.DataSource
{
    /// <summary>
    /// Class MovieService.
    /// Implements the <see cref="Contract.Interfaces.ICaller" />
    /// </summary>
    /// <seealso cref="Contract.Interfaces.ICaller" />
    public class MovieService : ICaller
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private static string _connectionString = null;
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                if (!String.IsNullOrEmpty(_connectionString))
                    return _connectionString;
                return String.Empty;
            }
            set
            {
                _connectionString = value;
            }
        }

        // KLASA TA PELNI FUNKCJE SERVISU WYKONUJACEGO METODY ROBIACE OPERACJE NA BAZIE DANYCH
        /// <summary>
        /// Edits the movie.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool EditMovie(Movie movie)
        {
            using (var context = new ETLModel(ConnectionString))
            {
                try
                {
                    // POBIERAMY FILM Z BAZY DANYCH I PODMIENIAMY JEGO DANE
                    var movietoEdit = context.Movies.SingleOrDefault(x => x.MovieId == movie.MovieId);
                    if (movietoEdit == null) return false; 

                    movietoEdit.Title = movie.Title;
                    movietoEdit.OrginalTitle = movie.OrginalTitle;
                    movietoEdit.Rank = movie.Rank;
                    movietoEdit.RateTotalVotes = movie.RateTotalVotes;
                    movietoEdit.Rate = movie.Rate;
                    movietoEdit.Year = movie.Year;
                    movietoEdit.Description = movie.Description;
                    movietoEdit.Director = movie.Director;
                    movietoEdit.Duration = movie.Duration;
                    movietoEdit.BoxOffice = movie.BoxOffice;

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

        /// <summary>
        /// Adds the movies.
        /// </summary>
        /// <param name="movies">The movies.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        public bool AddMovies(List<Movie> movies)
        {
            using (var context = new ETLModel(ConnectionString))
            {
                // LAZY LOADING POZWALA NA ZWIEKSZENIE SZYBKOSCI POBIERANIA DANYCH Z BAZY ZA POMOCA ENTITY FRAMEWORKA
                // NIE POBIERA ON OBJEKTOW REFEJENCYJNYCH (np. POBIERAJAC FILM NIE POBIERZE NIEPOTRZEBNIE AUTOMATYCZNIE WSZYSTKICH AKTOROW)
                context.Configuration.LazyLoadingEnabled = false;

                try
                {
                    // NA SAMYM POCZATKU POBIERAMY DANE JUZ ZAWARTE W BAZIE, BY POTEM JE SPRAWDZAC I ZAPOBIERZ REDUNDANCJI
                    var moviesDB = context.Movies.AsNoTracking().ToList();
                    var persons = context.Persons.AsNoTracking().ToList();
                    var types = context.MovieTypes.AsNoTracking().ToList();

                    foreach (var movie in movies)
                    {
                        // SPRAWDZAMY CZY FILM JUZ ISTNIEJE W BAZIE
                        if (moviesDB.Any(x => x.OrginalTitle.Contains(movie.OrginalTitle) || x.Title.Contains(movie.Title)))
                            continue;

                        var movieDb = context.Movies.Create();
                        movieDb.BoxOffice = movie.BoxOffice;
                        movieDb.Description = movie.Description;
                        movieDb.Director = movie.Director;
                        movieDb.Duration = movie.Duration;
                        movieDb.OrginalTitle = movie.OrginalTitle;
                        movieDb.Title = movie.Title;
                        movieDb.Rank = movie.Rank;
                        movieDb.Year = movie.Year;
                        movieDb.RateTotalVotes = movie.RateTotalVotes;
                        movieDb.Rate = movie.Rate;
                        movieDb.ReleaseDate = movie.ReleaseDate;
                        movieDb.Production = (byte)movie.Production;
                        movieDb.Staff = new List<PersonDto>();
                        movieDb.Types = new List<MovieTypeDto>();

                        // PETLA KTORA ITERUJE PO AKTORACH, JESLI NIE MA GO W BAZIE TO GO DODA
                        // W INNYM PRZYPADKU DOPISZE GO DO FILMU
                        foreach (var a in movie.Staff)
                        {
                            if (!persons.Any(x => x.Name == a.Name && x.Surname == a.Surname))
                            {
                                var person = context.Persons.Create();
                                person.Name = a.Name;
                                person.Surname = a.Surname;
                                context.Persons.Add(person);
                                context.SaveChanges();
                                persons.Add(person);
                                movieDb.Staff.Add(person);
                            }
                            else
                            {
                                movieDb.Staff.Add(persons.Where(x => x.Name == a.Name && x.Surname == x.Surname).FirstOrDefault());
                            }          
                        }

                        // TO SAMO JAK Z AKTORAMI
                        foreach (var t in movie.Types)
                        {
                            if (!types.Any(x => x.Name == t.Name))
                            {
                                var type = context.MovieTypes.Create();
                                type.Description = t.Description;
                                type.Type = (byte)t.Type;
                                type.Name = t.Name;
                                context.MovieTypes.Add(type);
                                context.SaveChanges();
                                types.Add(type);
                                movieDb.Types.Add(type);
                            }
                            else
                            {
                                movieDb.Types.Add(types.Where(x => x.Name == t.Name).FirstOrDefault());
                            }
                        }

                        context.Movies.Add(movieDb);
                        context.SaveChanges();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Gets all movies.
        /// </summary>
        /// <returns>List&lt;Movie&gt;.</returns>
        /// <exception cref="Exception"></exception>
        public List<Movie> GetAllMovies()
        {
            using (var context = new ETLModel(ConnectionString))
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

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        public bool RemoveAll()
        {
            using (var context = new ETLModel(ConnectionString))
            {
                try
                {
                    // USUAWNIE WSZYSTKICH REKORDÓW Z BAZY DANYCH
                    var movies = context.Movies.ToList();
                    context.Movies.RemoveRange(movies);

                    //var persons = context.Persons.ToList();
                    //context.Persons.RemoveRange(persons);

                    //var types = context.MovieTypes.ToList();
                    //context.MovieTypes.RemoveRange(types);

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

        /// <summary>
        /// Determines whether [is server connected].
        /// </summary>
        /// <returns><c>true</c> if [is server connected]; otherwise, <c>false</c>.</returns>
        public bool IsServerConnected()
        {
            using (var context = new ETLModel(ConnectionString))
            {
                try
                {
                    context.Database.Connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
