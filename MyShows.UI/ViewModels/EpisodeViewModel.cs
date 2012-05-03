using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Caliburn.Micro;
using MyShows.Core;
using UTorrentAPI;

namespace MyShows.UI.ViewModels
{
    
    public class EpisodeViewModel
    {
        private readonly Episode _episode;
        private static UTorrentClient _client = new UTorrentClient(new Uri( "http://localhost:8081/gui/"), "admin", "dawoo");
        private  BindableCollection<TorrentViewModel> _torrents;

        public EpisodeViewModel(Episode episode)
        {
            _episode = episode;
            _torrents = new BindableCollection<TorrentViewModel>(episode.Torrents.Select(t=>new TorrentViewModel(t,_client)));
        }

        public string CodedName
        {
            get { return _episode.CodedName; }
        }

        public string Title
        {
            get { return _episode.Title; }
        }

        public BindableCollection<TorrentViewModel> Torrents { get { return _torrents; } }

        public void Download()
        {
            Torrents.First().Download();
        }

        public void Play()
        {
            var torrent = Torrents.Where(t=>t.IsDownloaded).FirstOrDefault();
            if (torrent == null)
                return;
            Process.Start(torrent.File);
        }

        public void SaveSubtitles()
        {
            
        }
    }
}