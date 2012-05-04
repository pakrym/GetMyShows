using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyShows.Core
{
    public enum TorrentStateStatus
    {
        Unknown,
        NotFound,
        Downloading,
        Done
    }

    public class TorrentState : INotifyPropertyChanged
    {
        private int _season;
        private int _number;
        public TorrentState(string hash, int season, int number)
        {
            Hash = hash;
            _season = season;
            _number = number;
        }



        internal string Hash { get; set; }

        private UTorrentAPI.Torrent _torrent;
        private UTorrentAPI.File _file;

        internal void UpdateTorrent(UTorrentAPI.Torrent torrent)
        {
            if (torrent == null)
            {
                Status = TorrentStateStatus.NotFound;
                return;
            }
            _torrent = torrent;
            Status = TorrentStateStatus.Downloading;
            _file = FindFile(torrent);
            if (_file != null)
            {
                FilePath = _file.Path;
                if (_file.SizeInBytes == _file.DownloadedBytes) Status = TorrentStateStatus.Done;
                

            }

        }

        private TorrentStateStatus _status;
        public TorrentStateStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private UTorrentAPI.File FindFile(UTorrentAPI.Torrent torrent)
        {
            var cn = Episode.EncodeName(_season, _number);
            return torrent.Files.Where(f => f.Path.IndexOf(cn, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
