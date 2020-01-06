// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 12-30-2019
// ***********************************************************************
// <copyright file="MovieTypeDto.cs" company="">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Dtos
{
    /// <summary>
    /// Class MovieTypeDto.
    /// </summary>
    [Table("MovieTypes")]
    public class MovieTypeDto
    {
        /// <summary>
        /// Gets or sets the movie type identifier.
        /// </summary>
        /// <value>The movie type identifier.</value>
        [Key]
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
        public byte Type { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the movies.
        /// </summary>
        /// <value>The movies.</value>
        public virtual ICollection<MovieDto> Movies { get; set; }
    }
}
