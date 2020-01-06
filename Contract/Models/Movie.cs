using Contract.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contract.Model
{
    public class Movie
    {
        // KLASA MOVIE KTORA UZYWANA JEST DO PREZENTACJI NA FRONCIE
        // ROZNI SIE O KLASY BAZODANOWEJ TYM ZE TUTAL MOZNA OKRESLAC ROZNE TYPY TAKIE JAK ENUM CZY DEFINIOWAC NOWE POLA KTORE ZWRACAJA OKRESLONE DANE (np. TypesString)

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string OrginalTitle { get; set; }
        public string Director { get; set; }
        public int? Rank { get; set; }
        public int? Year { get; set; }
        public string Duration { get; set; }
        public int? Rate { get; set; }
        public int? RateTotalVotes { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal BoxOffice { get; set; }
        public PRODUCTION_COUNTRY Production { get; set; }
        public List<MovieType> Types { get; set; }
        public List<Person> Staff { get; set; }
        // ZAWIARA ONA LISTE AKTOROW I GATUNKOW

        public string StaffString
        {
            get
            {
                if (Staff == null || Staff.Count == 0) return null;
                else return string.Join(", ", Staff.Select(x => x.NameSurname).ToArray());
            }
        }

        public string TypesString
        {
            get
            {
                if (Types == null || Types.Count == 0) return null;
                else return string.Join(", ", Types.Select(x => x.Name).ToArray());
            }
        }

        public string TitleString
        {
            get
            {
                if (Title == null) return OrginalTitle;
                else return Title;
            }
        }
    }
}
