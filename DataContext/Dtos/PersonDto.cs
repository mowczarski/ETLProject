using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Dtos
{
    [Table("Persons")]
    public class PersonDto
    {
        [Key]
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<MovieDto> Movies { get; set; }
    }
}
