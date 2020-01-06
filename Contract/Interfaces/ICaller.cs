using Contract.Model;
using System.Collections.Generic;


namespace Contract.Interfaces
{
    public interface ICaller
    {
        //JEST TO ZWYKLY INTERFEJS KTORY OKRESLA MEDOTY WYMAGANE PODCZAS JEGO IMPLEMENTACJI

        #region interface methods
        string ConnectionString { get; set; }

        bool EditMovie(Movie movie);

        bool AddMovies(List<Movie> movie);

        List<Movie> GetAllMovies();

        bool RemoveAll();

        bool IsServerConnected();
        #endregion
    }
}
