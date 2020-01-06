// ***********************************************************************
// Assembly         : Contract
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="Person.cs" company="Contract">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Contract.Model
{
    /// <summary>
    /// Class Person.
    /// </summary>
    public class Person
    {
        //KLASA AKTOR
        /// <summary>
        /// Gets or sets the person identifier.
        /// </summary>
        /// <value>The person identifier.</value>
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
        /// Gets the name surname.
        /// </summary>
        /// <value>The name surname.</value>
        public string NameSurname 
        { 
            get
            {
                return Name + " " + Surname;
            }
        }
    }
}
