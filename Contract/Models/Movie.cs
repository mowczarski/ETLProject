// ***********************************************************************
// Assembly         : Contract
// Author           : Mariusz
// Created          : 12-30-2019
//
// Last Modified By : Mariusz
// Last Modified On : 01-06-2020
// ***********************************************************************
// <copyright file="Movie.cs" company="Contract">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contract.Model
{
    /// <summary>
    /// Class Movie.
    /// </summary>
    public class Movie
    {
        // KLASA MOVIE KTORA UZYWANA JEST DO PREZENTACJI NA FRONCIE
        // ROZNI SIE O KLASY BAZODANOWEJ TYM ZE TUTAL MOZNA OKRESLAC ROZNE TYPY TAKIE JAK ENUM CZY DEFINIOWAC NOWE POLA KTORE ZWRACAJA OKRESLONE DANE (np. TypesString)

        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>The movie identifier.</value>
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
        public PRODUCTION_COUNTRY Production { get; set; }
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>The types.</value>
        public List<MovieType> Types { get; set; }
        /// <summary>
        /// Gets or sets the staff.
        /// </summary>
        /// <value>The staff.</value>
        public List<Person> Staff { get; set; }
        // ZAWIARA ONA LISTE AKTOROW I GATUNKOW

        /// <summary>
        /// Gets the staff string.
        /// </summary>
        /// <value>The staff string.</value>
        public string StaffString
        {
            get
            {
                if (Staff == null || Staff.Count == 0) return null;
                else return string.Join(", ", Staff.Select(x => x.NameSurname).ToArray());
            }
        }

        /// <summary>
        /// Gets the types string.
        /// </summary>
        /// <value>The types string.</value>
        public string TypesString
        {
            get
            {
                if (Types == null || Types.Count == 0) return null;
                else return string.Join(", ", Types.Select(x => x.Name).ToArray());
            }
        }

        /// <summary>
        /// Gets the title string.
        /// </summary>
        /// <value>The title string.</value>
        public string TitleString
        {
            get
            {
                if (Title == null) return OrginalTitle;
                else return Title;
            }
        }
    }
}
