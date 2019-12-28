using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL.Webscraper
{
    public class MovieModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public List<Actor> Actors { get; set; }

        public MovieModel(string cTitle, string cDescription, string cDirector, List<Actor> cActors)
        {
            Title = cTitle;
            Description = cDescription;
            Director = cDirector;
            Actors = cActors;
        }

        override public string ToString(){
            return Title;
        }
    }

    public class Actor
    {
        public string Name { get; set; }

        public Actor(string cName)
        {
            Name = cName;
        }

        override public string ToString(){
            return Name;
        }
    }
}
