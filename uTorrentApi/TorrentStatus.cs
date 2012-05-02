// -----------------------------------------------------------------------
// <copyright file="TorrentStatus.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
// -----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;

    /// <summary>
    /// Designates the current status of the torrent
    /// </summary>
    [Flags]
    public enum TorrentStatus
    {
        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        Started = 1,

        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        Checking = 2,

        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        StartAfterCheck = 4,

        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        Checked = 8,

        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        Error = 16,

        /// <summary>
        /// Paused means the torrent job is paused, but not stopped (still connected to peers). 
        /// </summary>
        Paused = 32,

        /// <summary>
        /// Queued means the torrent job is waiting for another torrent job to finish downloading before starting.
        /// </summary>
        Queued = 64,

        /// <summary>
        /// DOCUMENTATION NEEDED
        /// </summary>
        Loaded = 128,

        /// <summary>
        /// <para><c>FinishedOrStopped</c> means the torrent job is not uploading.  If progress = 1000, then
        /// the torrent is Finished and has been stopped after it reached seeding mode.  If Progress &lt; 1000, 
        /// then the torrent was stopped.</para>
        /// <para>If the torrent job reaches Finished without user intervention, then it means it reached 
        /// the seeding goal.</para>
        /// </summary>
        FinishedOrStopped = 136,
    }
}