using System.Collections.Generic;
using System.Diagnostics;
using Db4objects.Db4o.Collections;

namespace MyShows.Core.Models
{

    [DebuggerDisplay("{Name} Episodes: {Episodes.Count}")]
    public class Series
    {
        public Series():this(null)
        {
        }

        public Series(string seriesName)
        {
            Name = seriesName;
            Episodes = new ActivatableList<Episode>();
        }

        public string Name { get; set; }
        public ActivatableList<Episode> Episodes { get; set; }
    }
}