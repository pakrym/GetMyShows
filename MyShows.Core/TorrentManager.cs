using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using UTorrentAPI;

namespace MyShows.Core
{
    class TorrentManager
    {
        private List<WeakReference> _tracked;

        private readonly UTorrentClient _client;
        private Timer _timer;

        public TorrentManager(string url, string user, string password)
        {
            _tracked = new List<WeakReference>();
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += () => Update();
            _timer.Enabled = true;
        }


        public TorrentState CreateTorrentState(Torrent t)
        {
            var hash = ExtractHash(t.Magnet);

            var torrent = FindTorrent(hash);

            var ts = new TorrentState(hash, t.Episode.Season, t.Episode.Number);

            _tracked.Add(new WeakReference(ts));
            return ts;
        }

        public void Update()
        {
            try
            {
                _client = new UTorrentClient(new Uri(url), user, password);
            }
            catch (Exception ex)
            {
                _client.Torrents.Update();
            }
            
            foreach (var r in _tracked)
            {
                if (r.IsAlive)
                {
                    var torrentState = (TorrentState)r.Target;
                    torrentState.UpdateTorrent(_client.Torrents[torrentState.Hash]);
                }
            }

            _tracked.RemoveRange(_tracked.Where(r=>r.IsAlive);
        }

        private string ExtractHash(string magnet)
        {
            var m = Regex.Match(magnet, "btih:([0-9a-fA-f]{40})");
            if (!m.Groups[1].Success) return null;
            return m.Groups[1].Value;
        }

        private UTorrentAPI.Torrent FindTorrent(string btih)
        {
            return
                _client.Torrents.Where(t => StringComparer.OrdinalIgnoreCase.Compare(t.Hash, btih) == 0).FirstOrDefault();
        }

    }
}
