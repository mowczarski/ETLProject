using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContext.Dtos
{
    [Table("MovieTypes")]
    public class MovieTypeDto
    {
        [Key]
        public int MovieTypeId { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MovieDto> Movies { get; set; }
    }
}
