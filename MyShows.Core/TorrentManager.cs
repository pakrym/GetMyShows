using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTorrentAPI;

namespace MyShows.Core
{
    class TorrentManager
    {
        private List<WeakReference<TorrentState>> _tracked
        private readonly UTorrentClient _client;
        private File _file;
        public TorrentManager(string url, string user, string password)
        {
            
        }

        private readonly UTorrentClient _client;
        
        public TorrentState CreateTorrentState(Episode e)
        {
        }

        private UTorrentAPI.Torrent FindTorrent(string magnet)
        {
            var m = Regex.Match(magnet, "btih:([0-9a-fA-f]{40})");
            if (!m.Groups[1].Success) return null;
            var btih = m.Groups[1].Value;
            return
                _client.Torrents.Where(t => StringComparer.OrdinalIgnoreCase.Compare(t.Hash, btih) == 0).FirstOrDefault();
        }

        private File FindFile(Core.Episode episode, UTorrentAPI.Torrent torrent)
        {
            if (torrent == null) return null;
            else
            {
                return torrent.Files.Where(f => f.Path.IndexOf(episode.CodedName, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
            }
        }
    }
}
