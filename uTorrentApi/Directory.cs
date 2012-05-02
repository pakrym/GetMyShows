//-----------------------------------------------------------------------
// <copyright file="Directory.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    /// <summary>
    /// A directory that uTorrent can save to.
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// Initializes a new instance of the Directory class.
        /// </summary>
        /// <param name="path">the path that is used for saving</param>
        /// <param name="availableMBytes">the space available in this directory</param>
        internal Directory(string path, int availableMBytes)
        {
            this.Path = path;
            this.AvailableMBytes = availableMBytes;
        }

        /// <summary>
        /// Gets the path that is used for saving
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the space available in this directory
        /// </summary>
        public int AvailableMBytes { get; private set; }
    }
}
