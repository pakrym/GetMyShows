using System;
using System.Linq;
using System.Text.RegularExpressions;
using MyShows.Core;
using UTorrentAPI;
using Torrent = MyShows.Core.Torrent;

namespace MyShows.UI.ViewModels
{
    public class TorrentViewModel
    {
        public static readonly TorrentManager TorrentManager = new TorrentManager("http://localhost:8081/gui/","admin","dawoo");

        private readonly Torrent _torrentModel;
        
        private string _savePath;

        public TorrentViewModel(Torrent torrentModel, UTorrentClient client)
        {
            _torrentModel = torrentModel;
            //_client = client;
            _savePath = PathManager.GetTorrentPathForEpisode(_torrentModel.Episode);
            State = TorrentManager.CreateTorrentState(torrentModel);
        }

      

        public string Title { get { return _torrentModel.Title; } }
        public TorrentState State { get; private set; }

        public string File { 
            get {
                if (State.FilePath == null) return null;
                return System.IO.Path.Combine(_savePath, State.FilePath); 
            } 
        }

       
        //public bool IsDownloaded { get { return _torrent != null && ((_torrent.Status & TorrentStatus.FinishedOrStopped) > 0); } }

        public void Download()
        {
            if (State.Status != TorrentStateStatus.NotFound) return;

            if (!System.IO.Directory.Exists(_savePath)) System.IO.Directory.CreateDirectory(_savePath);
            TorrentManager.AddUrl(_torrentModel.Magnet, _savePath);
        }
    }
}