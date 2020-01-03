using Contract.Model;
using System.Collections.Generic;


namespace Contract.Interfaces
{
    public interface ICaller
    {
        #region interface methods
        bool AddMovie(Movie movie);

        bool AddMovies(List<Movie> movie);

        List<Movie> GetAllMovies();

        bool RemoveAll();
        #endregion
    }
}
