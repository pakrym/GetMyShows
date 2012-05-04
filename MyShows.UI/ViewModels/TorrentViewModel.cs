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
      //  private UTorrentAPI.Torrent _torrent;
        private string _savePath;

        public TorrentViewModel(Torrent torrentModel, UTorrentClient client)
        {
            _torrentModel = torrentModel;
            //_client = client;
            _savePath = PathManager.GetTorrentPathForEpisode(_torrentModel.Episode);
            
        }

      

        public string Title { get { return _torrentModel.Title; } }

        //public string File { 
        //    get {
        //        //if (_file == null) return null;
        //        return System.IO.Path.Combine(_savePath, _file.Path); 
        //    } 
        //}

       
        //public bool IsDownloaded { get { return _torrent != null && ((_torrent.Status & TorrentStatus.FinishedOrStopped) > 0); } }

        //public void Download()
        //{
        //    if (_torrent != null) return;
        //    if (!System.IO.Directory.Exists(_savePath)) System.IO.Directory.CreateDirectory(_savePath);
        //    _client.Torrents.AddUrl(_torrentModel.Magnet, _savePath);

        //    _torrent = FindTorrent(_torrentModel.Magnet);
        //}
    }
}