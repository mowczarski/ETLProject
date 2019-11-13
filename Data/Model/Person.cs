using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Persons")]
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool isActor { get; set; }
        public bool isDirector { get; set; }
        public bool isScenarist { get; set; }
        public bool isPhotographer { get; set; }
        public bool isComposer { get; set; }
        public bool isDescription { get; set; }
    }
}
