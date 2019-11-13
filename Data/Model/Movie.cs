using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string OrginalTitle { get; set; }
        public int? Rank { get; set; }
        public int? Year { get; set; }
        public string Duration { get; set; }
        public int? Rate { get; set; }
        public int? RateTotalVotes { get; set; }
        public string DistributionCompany { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Studio { get; set; }
        public decimal BoxOffice { get; set; }
        public PRODUCTION_COUNTRY Production { get; set; }

        // wiele do wielu
        public virtual ICollection<MovieType> Types { get; set; }
        public virtual ICollection<Person> Staff { get; set; }

    }
}
