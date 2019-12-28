using DataContext.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Model
{
    [Table("MovieTypes")]
    public class MovieType
    {
        [Key]
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public MOVIE_TYPE Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
