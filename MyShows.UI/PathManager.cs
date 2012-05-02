using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MyShows.Core;

namespace MyShows.UI
{
    static class PathManager
    {
        public static string BasePath = "f:\\MORE\\MyShows\\";
        public static string GetTorrentPathForEpisode(Episode e)
        {
            return Path.Combine(BasePath, GetEpisodePart(e), "torrent\\");
        }

        private static string GetEpisodePart(Episode episode)
        {
            var path = string.Format("{0}\\{1}\\{2}", episode.Series.Title, episode.Season, episode.Number);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
