using DataContext.Model;
using MySql.Data.Entity;
using System;
using System.Data.Entity;
using System.Linq;

namespace DataContext.DataSource
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ETLModel : DbContext
    {
        public ETLModel() : base("name=ETLModel") { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<MovieType> MovieTypes { get; set; }
    }
}