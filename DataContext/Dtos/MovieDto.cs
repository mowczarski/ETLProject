using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Dtos
{
    [Table("Movies")]
    public class MovieDto
    {
        [Key]
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
        public byte Production { get; set; }

        // wiele do wielu
        public virtual ICollection<MovieTypeDto> Types { get; set; }
        public virtual ICollection<PersonDto> Staff { get; set; }

    }
}
