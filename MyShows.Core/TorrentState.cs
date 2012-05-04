using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShows.Core
{
    class TorrentState
    {
        private int _season;
        private int _number;
        public TorrentState(string hash,  int season, int number)
        {
            Hash = hash;
            _season = season;
            _number = number;
        }

        
        
        internal string Hash { get; set; }

        private UTorrentAPI.Torrent _torrent;
        private UTorrentAPI.File _file { get; set; }

        internal void UpdateTorrent(UTorrentAPI.Torrent torrent)
        {
            _torrent = torrent;
        }

        private UTorrentAPI.File FindFile(UTorrentAPI.Torrent torrent)
        {
            var cn = Episode.EncodeName(_season, _number);
            return torrent.Files.Where(f => f.Path.IndexOf(cn, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
        }
    }
}
