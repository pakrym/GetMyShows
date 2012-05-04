using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Db4objects.Db4o.Collections;
using MyShows.Core;
using NLog;

namespace MyShows.Update
{
    class Program
    {
        Dispatcher _dispatcher;

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        DataContext _context;
        ThePirateBaySearchProvider _tpb = new ThePirateBaySearchProvider();
        PodnapisiSearchProvider _podnapisi = new PodnapisiSearchProvider();
        private TimeSpan _updateTime = TimeSpan.FromHours(1);

        static void Main(string[] args)
        {
            bool cleanRun = false;
            new Program().Run(cleanRun);
        }

        void Run(bool cleanRun)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _context = new DataContext();

            var profiles = _context.GetProfiles();
            foreach (var profile in profiles)
            {
                if (cleanRun)
                {
                    foreach (var series in profile.Series.ToArray())
                    {
                        _context.RemoveSeries(series);
                    }
                    profile.Series.Clear();

                }
                new ApiParser(profile).ReadSeries();

                _context.Save();

                Parallel.ForEach(profile.Series.SelectMany(s => s.Episodes).Select(e=>e.EpisodeId), ProcessEpisode);

                _context.Save();
            }
        }

        private void ProcessEpisode(long episodeId)
        {
            DataContext context = new DataContext();
            var episode = context.GetEpisodeById(episodeId);

            if ((episode.LastUpdate - episode.AirDate).Days > 2) return;
            if (DateTime.Now - episode.LastUpdate < _updateTime) return;

            var series = episode.Series;

            var result = _tpb.Search(series, episode).Take(3);

            foreach (var torrent in episode.Torrents.ToArray())
            {
                _context.RemoveTorrent(torrent);
            }

            foreach (var r in result)
            {
                _dispatcher.Invoke((Action)(() => episode.Torrents.Add(r)));
                _logger.Info("{0} [{2}] {1}", r.Title, r.Magnet.Substring(0, 10), r.Seed);
            }
            var subs = _podnapisi.Search(series, episode);

            foreach (var subtitles in episode.Subtitles.ToArray())
            {
                _dispatcher.Invoke((Action)(() => _context.RemoveSubtitles(subtitles)));
            }

            foreach (var s in subs)
            {
                _dispatcher.Invoke((Action)(() => episode.Subtitles.Add(s)));
                _logger.Info("{0} {1}", s.Title, s.File.Length);
            }

            _dispatcher.Invoke((Action)(() => episode.LastUpdate = DateTime.Now));
            context.Save();
        }
    }
}
