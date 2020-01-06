// ***********************************************************************
// Assembly         : Contract
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-04-2020
// ***********************************************************************
// <copyright file="MovieType.cs" company="Contract">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Contract.Enum;

namespace Contract.Model
{
    /// <summary>
    /// Class MovieType.
    /// </summary>
    public class MovieType
    {
        // KLASA GATUNKOW FILMOW
        /// <summary>
        /// Gets or sets the movie type identifier.
        /// </summary>
        /// <value>The movie type identifier.</value>
        public int MovieTypeId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public MOVIE_TYPE Type { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
    }
}
