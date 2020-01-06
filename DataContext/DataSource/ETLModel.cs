// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="ETLModel.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using DataContext.Dtos;
using MySql.Data.Entity;
using System.Data.Entity;
using System.Data.SqlClient;

namespace DataContext.DataSource
{
    // JEST TO KLASA KTORA DZIEDZICZY PO DbContext
    // UMOZLIWIA TO ZASTOSOWANIE ENTITY FRAMEWORK DO KOMUNIKACJI Z BAZA DANYCH

    /// <summary>
    /// Class ETLModel.
    /// Implements the <see cref="System.Data.Entity.DbContext" />
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ETLModel : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ETLModel"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ETLModel(string value) : base(value)
        {
            ////this.Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Gets or sets the movies.
        /// </summary>
        /// <value>The movies.</value>
        public DbSet<MovieDto> Movies { get; set; }
        /// <summary>
        /// Gets or sets the persons.
        /// </summary>
        /// <value>The persons.</value>
        public DbSet<PersonDto> Persons { get; set; }
        /// <summary>
        /// Gets or sets the movie types.
        /// </summary>
        /// <value>The movie types.</value>
        public DbSet<MovieTypeDto> MovieTypes { get; set; }
    }
}