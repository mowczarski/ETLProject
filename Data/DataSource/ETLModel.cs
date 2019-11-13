using Data.Model;
using System;
using System.Data.Entity;
using System.Linq;

namespace Data.DataSource
{

    public class ETLModel : DbContext
    {
        public ETLModel() : base("name=ETLModel") { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Movie_Person> Movies_Persons { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<MovieType> MovieTypes { get; set; }
    }
}