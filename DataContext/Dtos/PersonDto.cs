// ***********************************************************************
// Assembly         : DataContext
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 12-30-2019
// ***********************************************************************
// <copyright file="PersonDto.cs" company="">
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
    /// Class PersonDto.
    /// </summary>
    [Table("Persons")]
    public class PersonDto
    {
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
        [Key]
        public int PersonId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>The surname.</value>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the movies.
        /// </summary>
        /// <value>The movies.</value>
        public virtual ICollection<MovieDto> Movies { get; set; }
    }
}
