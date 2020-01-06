// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="MovieDto.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Dtos
{
    /// <summary>
    /// Class MovieDto.
    /// </summary>
    [Table("Movies")]
    public class MovieDto
    {
        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>The movie identifier.</value>
        [Key]
        public int MovieId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the orginal title.
        /// </summary>
        /// <value>The orginal title.</value>
        public string OrginalTitle { get; set; }
        /// <summary>
        /// Gets or sets the director.
        /// </summary>
        /// <value>The director.</value>
        public string Director { get; set; }
        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        /// <value>The rank.</value>
        public int? Rank { get; set; }
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public int? Year { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public string Duration { get; set; }
        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        /// <value>The rate.</value>
        public int? Rate { get; set; }
        /// <summary>
        /// Gets or sets the rate total votes.
        /// </summary>
        /// <value>The rate total votes.</value>
        public int? RateTotalVotes { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the release date.
        /// </summary>
        /// <value>The release date.</value>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Gets or sets the box office.
        /// </summary>
        /// <value>The box office.</value>
        public decimal BoxOffice { get; set; }
        /// <summary>
        /// Gets or sets the production.
        /// </summary>
        /// <value>The production.</value>
        public byte Production { get; set; }

        // wiele do wielu
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public virtual ICollection<MovieTypeDto> Types { get; set; }
        /// <summary>
        /// Gets or sets the staff.
        /// </summary>
        /// <value>The staff.</value>
        public virtual ICollection<PersonDto> Staff { get; set; }

    }
}
