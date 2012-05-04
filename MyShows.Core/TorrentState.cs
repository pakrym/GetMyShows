using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShows.Core
{
    class TorrentState
    {
        public TorrentState(string hash, string file)
        {
            File = file;
            Hash = hash;
        }

        internal string File { get; private set; }
        internal string Hash { get; private set; }
        private UTorrentAPI.Torrent _torrent;

        internal void UpdateTorrent(Torrrent  torrent)
        {
            _torrent = torrent;
        }
    }
}
