using System;
using System.Linq;
using System.Text.RegularExpressions;
using UTorrentAPI;
using Torrent = MyShows.Core.Torrent;

namespace MyShows.UI.ViewModels
{
    public class TorrentViewModel
    {
        private readonly Torrent _torrentModel;
        private readonly UTorrentClient _client;
        private UTorrentAPI.Torrent _torrent;

        public TorrentViewModel(Torrent torrentModel, UTorrentClient client)
        {
            _torrentModel = torrentModel;
            _client = client;

            _torrent = FindTorrent(torrentModel.Magnet);
        }

        private UTorrentAPI.Torrent FindTorrent(string magnet)
        {
            var m = Regex.Match(magnet, "btih:([0-9a-fA-f]{40})");
            if (!m.Groups[1].Success) return null;
            var btih = m.Groups[1].Value;
            return
                _client.Torrents.Where(t => StringComparer.OrdinalIgnoreCase.Compare(t.Hash, btih) == 0).FirstOrDefault();
        }

        public bool IsDownloaded { get { return _torrent != null && ((_torrent.Status & TorrentStatus.FinishedOrStopped) > 0); } }
        
        public void Download()
        {
            if (_torrent != null ) return;
     
            _client.Torrents.AddUrl(_torrentModel.Magnet, PathManager.GetTorrentPathForEpisode(_torrentModel.Episode));

            _torrent = FindTorrent(_torrentModel.Magnet);
        }
    }
}