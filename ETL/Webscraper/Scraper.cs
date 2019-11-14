using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ETL.Webscraper
{
    public class FilmWebScraper
    {
        private readonly string FilmWebUrl = "https://www.filmweb.pl";
        private readonly string MoviesListSuffix = "/films/search?orderBy=popularity&descending=true&page=";
        private readonly string CastSuffix = "/cast/actors";
        private readonly HtmlWeb HtmlWeb = new HtmlWeb();

        private int counter = 0;

        public List<MovieModel> ScrapeMovies(int from, int to)
        {
            var resultMovies = new List<MovieModel>();
            for (int i = from; i < to; i++){
                var page = HtmlWeb.Load(FilmWebUrl + MoviesListSuffix + i).DocumentNode;
                var movies = page.SelectNodes("//*[@class = 'hits__item']");
     
                foreach (var movie in movies)
                {
                    Console.WriteLine("Scrapping: " + counter++);
                    var movieLinkNode = movie.SelectSingleNode(".//a[@class ='filmPreview__link']");
                    if (movieLinkNode != null){
                        resultMovies.Add(ScrapeMovie(HttpUtility.HtmlDecode(movieLinkNode.GetAttributeValue("href", "default"))));
                    } else {
                        System.Diagnostics.Debug.Print("Something wrong happenned on page: " + i + " after:" + resultMovies[resultMovies.Count-1]);
                    }
                    
                }            
            }
            Console.WriteLine("Return on: " + counter++);
            return resultMovies;
        }
        
        private MovieModel ScrapeMovie(string url)
        {
            var page = HtmlWeb.Load(FilmWebUrl + url).DocumentNode;
            
            return new MovieModel(
                getFilmTitle(page, url),
                getFilmDescription(page),
                getFilmDirector(page),
                getActors(url)
            );
        }

        private string getFilmTitle(HtmlNode page, string url)
        {
            return page.SelectSingleNode("//h1[@class = 'inline filmTitle']").SelectSingleNode("//a[@href = '" + url + "']")?.InnerText;
        }

        private string getFilmDescription(HtmlNode page)
        {
            var descriptionNode = page.SelectSingleNode("//div[@class = 'filmPlot bottom-15']");

            if (descriptionNode != null)
            {
                return descriptionNode.InnerText;
            } else
            {
                return "No description";
            }
        
        }

        private string getFilmDirector(HtmlNode page)
        {
            return page.SelectSingleNode("//li[@itemprop = 'director']")?.InnerText;
        }

        private List<Actor> getActors(String url)
        {
            var result = new List<Actor>();
            var page = HtmlWeb.Load(FilmWebUrl + url + CastSuffix).DocumentNode;
            var actorNodes = page.SelectNodes("//a[@rel = 'v:starring']");

            if (actorNodes != null)
            {
                foreach (var actor in actorNodes)
                {
                    result.Add(new Actor(actor.InnerText));
                }
            }

            return result;
        }
    }
}
