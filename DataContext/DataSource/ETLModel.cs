using DataContext.Dtos;
using MySql.Data.Entity;
using System.Data.Entity;

namespace DataContext.DataSource
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ETLModel : DbContext
    {
        public ETLModel() : base("name=ETLModel") { }

        public DbSet<MovieDto> Movies { get; set; }
        public DbSet<PersonDto> Persons { get; set; }
        public DbSet<MovieTypeDto> MovieTypes { get; set; }
    }
}