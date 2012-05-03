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
        private string _savePath;
        private File _file;

        public TorrentViewModel(Torrent torrentModel, UTorrentClient client)
        {
            _torrentModel = torrentModel;
            _client = client;
            _savePath = PathManager.GetTorrentPathForEpisode(_torrentModel.Episode);
            _torrent = FindTorrent(torrentModel.Magnet);
            _file = FindFile(_torrentModel.Episode, _torrent);
        }

        private File FindFile(Core.Episode episode, UTorrentAPI.Torrent torrent)
        {
            if (torrent == null) return null;
            else
            {
                return torrent.Files.Where(f => f.Path.IndexOf(episode.CodedName, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
            }
        }

        public string Title { get { return _torrentModel.Title; } }

        public string File { 
            get {
                if (_file == null) return null;
                return System.IO.Path.Combine(_savePath, _file.Path); 
            } 
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
            if (_torrent != null) return;
            if (!System.IO.Directory.Exists(_savePath)) System.IO.Directory.CreateDirectory(_savePath);
            _client.Torrents.AddUrl(_torrentModel.Magnet, _savePath);

            _torrent = FindTorrent(_torrentModel.Magnet);
        }
    }
}