// -----------------------------------------------------------------------
// <copyright file="TorrentRemovalOptions.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
// -----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;

    /// <summary>
    /// Used to specify what should be removed when removing a torrent
    /// </summary>
    [Flags]
    public enum TorrentRemovalOptions
    {
        /// <summary>
        /// Removes the selected torrent job(s) from the list, but all related files are left intact on the disk
        /// </summary>
        Job = 1,

        /// <summary>
        /// Removes the selected torrent job(s) from the list and the corresponding .torrent file(s) from the .torrent file storage location.
        /// </summary>
        TorrentFile = 3,
        
        /// <summary>
        /// Removes the selected torrent job(s) from the list and all content downloaded from the torrent job(s).
        /// </summary>
        Data = 5,
        
        /// <summary>
        /// Removes the selected torrent job(s) from the list, the corresponding .torrent file(s) from the .torrent file storage location, and all content downloaded from the torrent job(s).
        /// </summary>
        TorrentFileAndData = 7
    }
}