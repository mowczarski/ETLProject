using ETL.Helpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;

namespace ETL.Webscraper
{
    public class FilmWebScraper
    {
        #region fields
        private readonly string FilmWebUrl = "https://www.filmweb.pl";
        private readonly string MoviesListSuffix = "/films/search?orderBy=popularity&descending=true&page=";
        private readonly string CastSuffix = "/cast/actors";
        private readonly HtmlWeb HtmlWeb = new HtmlWeb();
        public event EventHandler<TextBoxValueEventArgs> TextBoxValueChanged;
        #endregion

        #region events
        // EVENTY W NASZYM PRZYPADKU SLUZA DO POWIADAMIANIA GLOWNEGO OKNA APLIKACJI ZEBY WYPISALA DO KONSOLI ZADANY TEKST
        protected virtual void OnTextBoxValueChanged(TextBoxValueEventArgs e)
        {
            TextBoxValueChanged?.Invoke(this, e);
        }

        public class TextBoxValueEventArgs : EventArgs
        {
            public string NewValue { get; set; }

            public TextBoxValueEventArgs(string newValue)
            {
                NewValue = newValue;
            }
        }
        #endregion

        public async Task<string> ScrapeMovies(int pageNumber)
        {
            List<dynamic> movieList = new List<dynamic>();
            string json = null;
            bool result = false;

            var page = HtmlWeb.Load(FilmWebUrl + MoviesListSuffix + pageNumber).DocumentNode;
            var movies = page.SelectNodes("//*[@class = 'hits__item']");

            if (movies == null || movies.Count == 0)
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    OnTextBoxValueChanged(new TextBoxValueEventArgs("Cannot get movies from filmweb.pl site !!"));
                }));
                return null;
            }

            foreach (var movie in movies)
            {
                var movieLinkNode = movie.SelectSingleNode(".//a[@class ='filmPreview__link']");

                if (movieLinkNode != null)
                {
                    result = ScrapeMovie(movieList, movie, HttpUtility.HtmlDecode(movieLinkNode.GetAttributeValue("href", "default")));
                }
                else
                {
                    await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        OnTextBoxValueChanged(new TextBoxValueEventArgs("Something wrong happenned on page: " + pageNumber));
                    }));
                }
            }

            // SERIALIZACJA DANYCH DO PLIKOW JSON
            if (result)
            {
                json = Newtonsoft.Json.JsonConvert.SerializeObject(movieList);
                var path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jsons";

                // TWORZENIE KATALOGU
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SetFolderPermission(path);
                System.IO.File.WriteAllText(path + $@"\movies_{pageNumber}.txt", json); // ZAPIS PLIKOW
            }

            return json;
        }

        public static void SetFolderPermission(string folderPath)
        {
            // W NIEKTORYCH PRZYPADKACH DOSTEP DO KATALOGU W KTORYM BEDA PLIKU JSON MOZE BYC ZABRONIONY
            // METODA TA NADAJE WSZYTSKIE UPRAWNIENIA DO TEGO KATALOGU
            var directoryInfo = new DirectoryInfo(folderPath);
            var directorySecurity = directoryInfo.GetAccessControl();
            var currentUserIdentity = WindowsIdentity.GetCurrent();
            var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name,
                                                          FileSystemRights.Read,
                                                          InheritanceFlags.ObjectInherit |
                                                          InheritanceFlags.ContainerInherit,
                                                          PropagationFlags.None,
                                                          AccessControlType.Allow);

            directorySecurity.AddAccessRule(fileSystemRule);
            directoryInfo.SetAccessControl(directorySecurity);
        }

        private bool ScrapeMovie(List<dynamic> movieList, HtmlNode mainPage, string url)
        {
            // GLOWNA METODA SKRAPUJACA FILMY
            var page = HtmlWeb.Load(FilmWebUrl + url).DocumentNode;
            dynamic movie = new ExpandoObject();
            try
            {
                movie.Title = getTitle(page, url);
                movie.OrginalTitle = getOrginalTitle(page);
                movie.Description = getDescription(page);
                movie.Director = getDirector(page);
                movie.Rank = getRank(page);
                movie.Year = getYear(mainPage);
                movie.Duration = getDuration(mainPage);
                movie.Rate = getRate(mainPage);
                movie.RateTotalVotes = getRateTotalVotes(mainPage);
                movie.ReleaseDate = getReleaseDate(mainPage, page);
                movie.BoxOffice = getBoxOffice(page);
                movie.Production = getProduction(page);
                movie.Types = getTypes(page);
                movie.Staff = getStaff(url);

                // WPISYWANIE DANYCH DO OKNA KONSOLI
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    OnTextBoxValueChanged(new TextBoxValueEventArgs($"Scrapping movie: {movie.Title} ({movie.OrginalTitle}," +
                   $" {movie.Year}) - director: {movie.Director} - rank: {movie.Rank} - rate {movie.Rate} (total: {movie.RateTotalVotes}) " +
                   $"- box office {movie.BoxOffice}"));
                }));

                movieList.Add(movie);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    OnTextBoxValueChanged(new TextBoxValueEventArgs("Error while scrapping: " + ex.Message));
                }));
                return false;
            }
            return true;
        }

        #region scraping methods
        private string getTitle(HtmlNode page, string url) => page.SelectSingleNode(".//h1[@class = 'inline filmTitle']").SelectSingleNode("//a[@href = '" + url + "']")?.InnerText;

        private string getOrginalTitle(HtmlNode page) => page.SelectSingleNode(".//h2[@class = 'cap s-16 top-5']")?.InnerText;

        private string getRank(HtmlNode page) => page.SelectSingleNode(".//a[@class = 'worldRanking']")?.InnerText;

        private string getYear(HtmlNode page) => page.SelectSingleNode(".//span[@class = 'filmPreview__year']")?.InnerText;

        private string getDuration(HtmlNode page) => page.SelectSingleNode(".//div[@class = 'filmPreview__filmTime']")?.InnerText;

        private string getRate(HtmlNode page) => page.SelectSingleNode(".//span[@class = 'rateBox__rate']")?.InnerText;

        private string getRateTotalVotes(HtmlNode page) => page.SelectSingleNode(".//span[@class = 'rateBox__votes rateBox__votes--count']")?.InnerText;

        private string getDescription(HtmlNode page) => page.SelectSingleNode(".//div[@class = 'filmPlot bottom-15']")?.InnerText;

        private string getDirector(HtmlNode page) => page.SelectSingleNode(".//li[@itemprop = 'director']")?.InnerText;

        private string getBoxOffice(HtmlNode page) => page.SelectSingleNode(".//li[@class = 'boxoffice']")?.InnerText;

        private string getProduction(HtmlNode page) => page.SelectSingleNode(".//a[@href = '/films/search?countries=53']")?.InnerText;

        private string getReleaseDate(HtmlNode mainPage, HtmlNode page)
        {
            var movieLinkNode = HttpUtility.HtmlDecode(mainPage.SelectSingleNode(".//a[@class ='filmPreview__link']").GetAttributeValue("href", "default")) + "/dates";
            return page.SelectSingleNode($"//a[@href = '{movieLinkNode}']")?.InnerText;
        }

        private List<dynamic> getStaff(String url)
        {
            var page = HtmlWeb.Load(FilmWebUrl + url + CastSuffix).DocumentNode;
            var actorNodes = page.SelectNodes("//a[@rel = 'v:starring']");
            List<dynamic> listOfStaffs = new List<dynamic>();

            if (actorNodes != null)
            {
                foreach (var actor in actorNodes.Take(5))
                {
                    dynamic staff = new ExpandoObject();
                    staff.Name = actor.InnerText.Split(' ')[0];
                    staff.Surname = actor.InnerText.Split(' ')[1];
                    listOfStaffs.Add(staff);
                }
            }
            return listOfStaffs;
        }

        private List<dynamic> getTypes(HtmlNode page)
        {
            List<dynamic> listOfTypes = new List<dynamic>();

            var typeNodes = page.SelectSingleNode("//div[@class = 'filmPlot bottom-15']").SelectSingleNode("//ul[@class = 'inline sep-comma genresList']").SelectNodes(".//li");

            if (typeNodes != null)
            {
                foreach (var t in typeNodes)
                {
                    dynamic type = new ExpandoObject();
                    type.Name = t.InnerText;
                    type.Type = Converters.ConvertToTypeByte(t.InnerText);
                    type.Description = "-";
                    listOfTypes.Add(type);
                }
            }
            return listOfTypes;
        }
        #endregion
    }
}
