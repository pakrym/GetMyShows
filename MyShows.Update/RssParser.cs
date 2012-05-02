using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyShows.Core;
using NLog;
using Newtonsoft.Json;

namespace MyShows.Update
{
    class ApiParser
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly Profile _profile;

        public ApiParser(Profile profile)
        {
            _profile = profile;
        }

        CookieContainer _cookieContainer = new CookieContainer();

        public void ReadSeries()
        {
            Authentication(_profile);


            IDictionary<int, RestShow> shows = null;
            IDictionary<int, RestEpisode> episodes = null;


            Parallel.Invoke(() => shows = GetShowsList(), () => episodes = GetUnwatchedEpisodesList());

            foreach (var restShow in shows.Values)
            {
                var series = _profile.Series.FirstOrDefault(p => p.Title == restShow.title);
                if (series == null)
                {
                    _profile.Series.Add(series = new Series(restShow.title));

                } 
                if (series.Image == null)
                {
                    series.Image = new WebClientEx().DownloadDataIgnoreAndLog(restShow.image);
                }
            }

            var allEpisodes = _profile.Series.SelectMany(s => s.Episodes).ToList();

            foreach (var restEpisode in episodes.Values.GroupBy(e => shows[e.showId]))
            {
                var series = _profile.Series.First(s => s.Title == restEpisode.Key.title);
                foreach (var episode in restEpisode)
                {
                    if (episode.episodeNumber == 0) continue; //skip specials
                    var ep =
                        allEpisodes.FirstOrDefault(
                            e => e.Season == episode.seasonNumber && e.Number == episode.episodeNumber);
                    if (ep != null)
                    {
                        ep.AirDate = DateTime.ParseExact( episode.airDate,"dd.MM.yyyy",CultureInfo.InvariantCulture);
                        allEpisodes.Remove(ep);
                        continue;
                    }


                    series.Episodes.Add(new Episode(episode.seasonNumber, episode.episodeNumber) { Title = episode.title });
                }
            }
            foreach (var episode in allEpisodes)
            {
                episode.Watched = true;
            }
        }

        class RestShow
        {
            public int showId { get; set; }
            public string title { get; set; }
            public string image { get; set; }
        }

        class RestEpisode
        {
            public int seasonNumber { get; set; }
            public int episodeNumber { get; set; }
            public string title { get; set; }
            public int showId { get; set; }
            public string airDate { get; set; }
        }

        private IDictionary<int, RestShow> GetShowsList()
        {
            _logger.Debug("Geting Shows List");
            var resp = ExecuteRequest("http://api.myshows.ru/profile/shows/");
            var text = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            return JsonConvert.DeserializeObject<IDictionary<int, RestShow>>(text);
        }

        private IDictionary<int, RestEpisode> GetUnwatchedEpisodesList()
        {

            _logger.Debug("Getting Unwatched Episodes List");
            var resp = ExecuteRequest("http://api.myshows.ru/profile/episodes/unwatched/");
            var text = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            return JsonConvert.DeserializeObject<IDictionary<int, RestEpisode>>(text);
        }

        private void Authentication(Profile profile)
        {
            _logger.Debug("Authentication");

            try
            {
                var url = string.Format(@"http://api.myshows.ru/profile/login?login={0}&password={1}", profile.Login,
                                        profile.Password);
                var resp = ExecuteRequest(url);
            }
            catch (WebException ex)
            {
                throw new InvalidOperationException("Auth error: " + ex);
            }
        }

        private HttpWebResponse ExecuteRequest(string url)
        {
            var req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            req.CookieContainer = _cookieContainer;
            return (HttpWebResponse)req.GetResponse();
        }
    }
    class RssParser
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void ReadSeries(Profile profile)
        {
            var url = string.Format("http://api.myshows.ru/rss/{0}/episodes/aired/", profile.UserId);

            var xml = XDocument.Parse(new WebClientEx().DownloadString(url));
            var channel = xml.Root.Element("channel");
            foreach (var item in channel.Elements("item"))
            {
                var title = item.Element("title").Value;
                var seriesName = title.Substring(0, title.LastIndexOf(' '));
                var episodeName = title.Substring(title.LastIndexOf(' ') + 1);

                var series = profile.Series.FirstOrDefault(s => s.Title == seriesName);
                if (series == null)
                    profile.Series.Add(series = new Series(seriesName));

                var episode = series.Episodes.FirstOrDefault(e => e.CodedName == episodeName);
                if (episode == null)
                    series.Episodes.Add(episode = new Episode(episodeName));
            }
        }
    }
}