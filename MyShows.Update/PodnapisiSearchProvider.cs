using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Zip;
using MyShows.Core;
using NLog;

namespace MyShows.Update
{
    class PodnapisiSearchProvider
    {
        private static string _root = "http://www.podnapisi.net";
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Subtitles[] Search(Series series, Episode episode)
        {

            var query =
                string.Format("http://www.podnapisi.net/ru/ppodnapisi/search?sJ=2&sT=1&sK={2}&sTS={0}&sTE={1}",
                              episode.Season, episode.Number, HttpUtility.UrlEncode(series.Title));
            List<Subtitles> subtitleses = new List<Subtitles>();
            var result = new WebClientEx().DownloadStringIgnoreAndLog(query);


            var aas = Regex.Matches(result, "class=\"subtitle_page_link\" href=\"([^\"]*?)\"").OfType<Match>().Select(m => m.Groups[1].Value).ToArray();

            return aas.Select(ProcessSubUrl).Where(r => r != null).AsParallel().ToArray();
        }

        private Subtitles ProcessSubUrl(string subUrl)
        {
            var subresult = new WebClientEx().DownloadStringIgnoreAndLog(_root + subUrl);
            if (string.IsNullOrWhiteSpace(subresult)) return null;
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(subresult);

                var href = _root + doc.DocumentNode.SelectSingleNode("//a[@class='button big download']").Attributes["href"].Value;

                var name = doc.DocumentNode.SelectSingleNode("//fieldset/legend[text()='Release']/../p/a").InnerText;

                var data = new WebClientEx().DownloadDataIgnoreAndLog(href);

                var outputStream = new MemoryStream();

                using (var zf = new ZipFile(new MemoryStream(data)))
                {
                    var ze = zf[0];
                    zf.GetInputStream(ze).CopyTo(outputStream);
                }

                return new Subtitles() { Title = name, File = outputStream.ToArray() };
            }
            catch { }

            return null;
        }
    }
}