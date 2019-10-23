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
    public class Scraper
    {

        private ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

        public ObservableCollection<EntryModel> Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }

        public void ScrapeData(string page)
        {
            var web = new HtmlWeb();
            var document = web.Load(page);

            var art = document.DocumentNode.SelectNodes("//*[@class = 'cat-prod-row-desc']");

            foreach ( var singleArt in art)
            {
                var header = HttpUtility.HtmlDecode(singleArt.SelectSingleNode(".//strong[@class ='cat-prod-row-name']").InnerText);
                var score = HttpUtility.HtmlDecode(singleArt.SelectSingleNode(".//span[@class ='prod-review']/span[@class ='product-score']").InnerText);
                Debug.Print(header);
            }
        }
    }
}
