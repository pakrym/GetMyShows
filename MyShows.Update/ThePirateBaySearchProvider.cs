using System.Collections.Generic;
using System.Web;
using HtmlAgilityPack;
using MyShows.Core;
using NLog;

namespace MyShows.Update
{
    class ThePirateBaySearchProvider
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Torrent[] Search(Series series, Episode episode)
        {
            var query = series.Title + " " + episode.CodedName;
            var url = string.Format("http://thepiratebay.se/search/{0}/0/7/0", HttpUtility.UrlEncode(query));

            var result = new WebClientEx().DownloadString(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result);
            var dts = doc.DocumentNode.SelectNodes("/html/body/div[@id='SearchResults']/div[@id='content']/div[@id='main-content']/table[@id='searchResult']/tr/td/div[@class='detName']");

            List<Torrent> results = new List<Torrent>();

            if (dts != null)
            {
                foreach (var dt in dts)
                {
                    var a = dt.ParentNode.SelectSingleNode("a");
                    var seed =
                        int.Parse(dt.ParentNode.ParentNode.SelectSingleNode("td[@align='right']").InnerText.Trim('\t', ' ', '\r',
                                                                                                                 '\n'));
                    results.Add(new Torrent() { Magnet = a.Attributes["href"].Value, Title = dt.InnerText.Trim('\t', ' ', '\r', '\n'), Seed = seed });
                }
            }

            return results.ToArray();
        }
    }
}