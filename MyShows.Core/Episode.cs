using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MyShows.Core
{
    public class DataContext
    {
        private DatabaseEntities _databaseEntities = new DatabaseEntities();

        public IEnumerable<Profile> GetProfiles()
        {
            return _databaseEntities.Profiles.ToList();
        }

        public void StoreProfile(Profile profile)
        {
            _databaseEntities.SaveChanges();
        }

        public void RemoveTorrent(Torrent torrent)
        {
            _databaseEntities.DeleteObject(torrent);
        }

        public void RemoveSubtitles(Subtitles subtitles)
        {
            _databaseEntities.DeleteObject(subtitles);
        }

        public IEnumerable<Series> GetUnwatchedSeries(int profileId)
        {
            return
                _databaseEntities.Profiles.First().Series.Where(
                    s => s.Episodes.Count(e => !e.Watched) > 0).ToList();
        }
    }

    public partial class Series
    {
        public Series()
        {
            
        }
        public Series(string title)
        {
            Title = title;
        }
    }

    [DebuggerDisplay("{CodedName}")]
    public partial class Episode
    {
        public Episode()
        {
            
        }
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
                var episode = episodeName.Substring(indexOfE + 1, episodeName.Length - indexOfE - 1);
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

        public string CodedName
        {
            get { return string.Format("s{0}e{1}", Season.ToString().PadLeft(2, '0'), Number.ToString().PadLeft(2, '0')); }
        }
    }
}
