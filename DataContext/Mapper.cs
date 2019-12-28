using Contract.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataContext.Dtos;
using Contract.Enum;

namespace DataContext
{
    public static class Mapper
    {
        public static Movie Get(this MovieDto item)
        {
            if (item == null) return null;

            return new Movie
            {
                MovieId = item.MovieId,
                Title = item.Title,
                OrginalTitle = item.OrginalTitle,
                Rank = item.Rank,
                Year = item.Year,
                Duration = item.Duration,
                Rate = item.Rate,
                RateTotalVotes = item.RateTotalVotes,
                DistributionCompany = item.DistributionCompany,
                Description = item.Description,
                ReleaseDate = item.ReleaseDate,
                Studio = item.Studio,
                BoxOffice = item.BoxOffice,
                Production = (PRODUCTION_COUNTRY)item.Production,
                Staff = item.Staff.ToList().Select(x => Get(x)).ToList(),
                Types = item.Types.ToList().Select(x => Get(x)).ToList(),
            };
        }

        public static MovieDto Set(this Movie item)
        {
            if (item == null) return null;

            return new MovieDto
            {
                MovieId = item.MovieId,
                Title = item.Title,
                OrginalTitle = item.OrginalTitle,
                Rank = item.Rank,
                Year = item.Year,
                Duration = item.Duration,
                Rate = item.Rate,
                RateTotalVotes = item.RateTotalVotes,
                DistributionCompany = item.DistributionCompany,
                Description = item.Description,
                ReleaseDate = item.ReleaseDate,
                Studio = item.Studio,
                BoxOffice = item.BoxOffice,
                Production = (byte)item.Production,
                Staff = item.Staff.ToList().Select(x => Set(x)).ToList(),
                Types = item.Types.ToList().Select(x => Set(x)).ToList(),
            };
        }

        public static Person Get(this PersonDto item)
        {
            if (item == null) return null;

            return new Person
            {
                PersonId = item.PersonId,
                Name = item.Name,
                Surname = item.Surname,
                isActor = item.isActor,
                isDirector = item.isDirector,
                isScenarist = item.isScenarist,
                isPhotographer = item.isPhotographer,
                isComposer = item.isComposer,
                isDescription = item.isDescription,
            };
        }

        public static PersonDto Set(this Person item)
        {
            if (item == null) return null;

            return new PersonDto
            {
                PersonId = item.PersonId,
                Name = item.Name,
                Surname = item.Surname,
                isActor = item.isActor,
                isDirector = item.isDirector,
                isScenarist = item.isScenarist,
                isPhotographer = item.isPhotographer,
                isComposer = item.isComposer,
                isDescription = item.isDescription,
            };
        }

        public static MovieTypeDto Set(this MovieType item)
        {
            if (item == null) return null;

            return new MovieTypeDto
            {
                MovieTypeId = item.MovieTypeId,
                Name = item.Name,
                Type = (byte)item.Type,
                Description = item.Description,
            };
        }

        public static MovieType Get(this MovieTypeDto item)
        {
            if (item == null) return null;

            return new MovieType
            {
                MovieTypeId = item.MovieTypeId,
                Name = item.Name,
                Type = (MOVIE_TYPE)item.Type,
                Description = item.Description,
            };
        }
    }
}


