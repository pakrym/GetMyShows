using System;
using System.Collections.Generic;
using System.Diagnostics;
using Db4objects.Db4o.Collections;

namespace MyShows.Core.Models
{
    [DebuggerDisplay("{Title} {Magnet}")]
    public class Torrent
    {
        public String Title { get; set; }
        public String Magnet { get; set; }
        public int Seed { get; set; }
    }


    [DebuggerDisplay("{Name} {Url}")]
    public class Subtitles
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    [DebuggerDisplay("{CodedName}")]
    public class Episode
    {
        public Episode(int season, int number)
        {
            Season = season;
            Number = number;
        }

        public Episode(string episodeName)
        {
            try
            {
                var indexOfE = episodeName.IndexOf('e');
                var season = episodeName.Substring(1, indexOfE - 1);
                var episode = episodeName.Substring(indexOfE+1, episodeName.Length - indexOfE-1);
                Season = int.Parse(season);
                Number = int.Parse(episode);
            }
            catch (FormatException)
            {
                throw new FormatException("Episode name format exception");
            }
            catch (IndexOutOfRangeException)
            {
                throw new FormatException("Episode name format exception");
            }

        }

        public int Season { get; set; }
        public int Number { get; set; }

        private ActivatableList<Torrent> _torrents;
        public ActivatableList<Torrent> Torrents
        {
            get { return _torrents?? (_torrents = new ActivatableList<Torrent>()); }
            set { _torrents = value; }
        }

        public string CodedName
        {
            get { return string.Format("s{0}e{1}", Season.ToString().PadLeft(2, '0'), Number.ToString().PadLeft(2, '0')); }
        }

        private ActivatableList<Subtitles> _subtitles;
        public ActivatableList<Subtitles> Subtitles
        {
            get { return _subtitles?? (_subtitles = new ActivatableList<Subtitles>()); }
            set { _subtitles = value; }
        }
    }
}