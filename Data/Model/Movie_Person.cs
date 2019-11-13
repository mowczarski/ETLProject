using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Movies_Persons")]
    public class Movie_Person
    {
        [Key]
        public int Movie_PersonId { get; set; }
        public Movie Movie { get; set; }
        public Person Person { get; set; }
        public string PlayedAs { get; set; }
    }
}
