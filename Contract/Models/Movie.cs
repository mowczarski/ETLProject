using Contract.Enum;
using System;
using System.Collections.Generic;

namespace Contract.Model
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string OrginalTitle { get; set; }
        public string Director { get; set; }
        public int? Rank { get; set; }
        public int? Year { get; set; }
        public string Duration { get; set; }
        public int? Rate { get; set; }
        public int? RateTotalVotes { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal BoxOffice { get; set; }
        public PRODUCTION_COUNTRY Production { get; set; }
        public List<MovieType> Types { get; set; }
        public List<Person> Staff { get; set; }
    }
}
