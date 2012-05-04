using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using UTorrentAPI;

namespace MyShows.Core
{
    public class TorrentManager
    {
        private readonly string _url;
        private readonly string _user;
        private readonly string _password;

        private List<WeakReference> _tracked;

        private UTorrentClient _client;
        private object _clientLock = new object();
        private Timer _timer;

        public TorrentManager(string url, string user, string password)
        {
            _url = url;
            _user = user;
            _password = password;
            _tracked = new List<WeakReference>();
            _timer = new Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += (s, e) => Update();
            _timer.Enabled = true;
        }


        public TorrentState CreateTorrentState(Torrent t)
        {
            var hash = ExtractHash(t.Magnet);


            var ts = new TorrentState(hash, t.Episode.Season, t.Episode.Number);

            _tracked.Add(new WeakReference(ts));
            return ts;
        }

        public void Update()
        {
            lock (_clientLock)
            {
                try
                {
                    _client = new UTorrentClient(new Uri(_url), _user, _password);
                }
                catch (Exception ex)
                {
                    // set  all to unknown state
                }

                foreach (var r in _tracked)
                {
                    if (r.IsAlive)
                    {
                        var torrentState = (TorrentState)r.Target;
                        if (_client.Torrents.Contains(torrentState.Hash))
                            torrentState.UpdateTorrent(_client.Torrents[torrentState.Hash]);
                        else
                        {
                            torrentState.UpdateTorrent(null);
                        }
                    }
                }

                _tracked.RemoveAll(wr => !wr.IsAlive);


            }
        }

        private string ExtractHash(string magnet)
        {
            var m = Regex.Match(magnet, "btih:([0-9a-fA-f]{40})");
            if (!m.Groups[1].Success) return null;
            return m.Groups[1].Value;
        }


        public void AddUrl(string magnet, string savePath)
        {
            lock (_clientLock)
            {
                _client.Torrents.AddUrl(magnet, savePath);
            }
            Update();
        }
    }
}
