using Contract.Model;
using System.Collections.Generic;


namespace Contract.Interfaces
{
    public interface ICaller
    {
        bool AddMovie(Movie movie);
        IList<Movie> GetAllMovies();
    }
}
