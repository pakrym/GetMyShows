using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Db4objects.Db4o.Collections;

using MyShows.Core;
using NLog;

namespace MyShows.Update
{
    class Program
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        DataContext _context = new DataContext();
        ThePirateBaySearchProvider _tpb = new ThePirateBaySearchProvider();
        PodnapisiSearchProvider _podnapisi = new PodnapisiSearchProvider();
        private TimeSpan _updateTime = TimeSpan.FromHours(1);

        static void Main(string[] args)
        {
            new Program().Run();
        }
        void Run()
        {
            var profiles = _context.GetProfiles();

            foreach (var profile in profiles)
            {
                new ApiParser(profile).ReadSeries();

                Parallel.ForEach(profile.Series.SelectMany(s => s.Episodes), ProcessEpisode);

                _context.StoreProfile(profile);
            }
        }

        private void ProcessEpisode(Episode episode)
        {
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
                episode.Torrents.Add(r);
                _logger.Info("{0} [{2}] {1}", r.Title, r.Magnet.Substring(0, 10), r.Seed);
            }
            var subs = _podnapisi.Search(series, episode);

            foreach (var subtitles in episode.Subtitles.ToArray())
            {
                _context.RemoveSubtitles(subtitles);
            }

            foreach (var s in subs)
            {
                episode.Subtitles.Add(s);
                _logger.Info("{0} {1}", s.Title, s.File.Length);
            }

            episode.LastUpdate = DateTime.Now;
        }
    }
}
