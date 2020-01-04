using DataContext.Dtos;
using MySql.Data.Entity;
using System.Data.Entity;

namespace DataContext.DataSource
{
    // JEST TO KLASA KTORA DZIEDZICZY PO DbContext
    // UMOZLIWIA TO ZASTOSOWANIE ENTITY FRAMEWORK DO KOMUNIKACJI Z BAZA DANYCH

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ETLModel : DbContext
    {
        public ETLModel() : base("name=ETLModel") {

            //this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<MovieDto> Movies { get; set; }
        public DbSet<PersonDto> Persons { get; set; }
        public DbSet<MovieTypeDto> MovieTypes { get; set; }
    }
}