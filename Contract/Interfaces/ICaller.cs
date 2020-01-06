// ***********************************************************************
// Assembly         : Contract
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="ICaller.cs" company="Contract">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Contract.Model;
using System.Collections.Generic;


namespace Contract.Interfaces
{
    /// <summary>
    /// Interface ICaller
    /// </summary>
    public interface ICaller
    {
        //JEST TO ZWYKLY INTERFEJS KTORY OKRESLA MEDOTY WYMAGANE PODCZAS JEGO IMPLEMENTACJI

        #region interface methods
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Edits the movie.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool EditMovie(Movie movie);

        /// <summary>
        /// Adds the movies.
        /// </summary>
        /// <param name="movie">The movie.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool AddMovies(List<Movie> movie);

        /// <summary>
        /// Gets all movies.
        /// </summary>
        /// <returns>List&lt;Movie&gt;.</returns>
        List<Movie> GetAllMovies();

        /// <summary>
        /// Removes all.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool RemoveAll();

        /// <summary>
        /// Determines whether [is server connected].
        /// </summary>
        /// <returns><c>true</c> if [is server connected]; otherwise, <c>false</c>.</returns>
        bool IsServerConnected();
        #endregion
    }
}
