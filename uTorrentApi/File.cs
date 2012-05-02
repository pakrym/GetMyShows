//-----------------------------------------------------------------------
// <copyright file="File.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    using UTorrentAPI.Protocol;

    /// <summary>
    /// Represents a file inside a torrent
    /// </summary>
    public class File
    {
        internal File(JsonArray json)
        {
            this.Path = json[0];
            this.SizeInBytes = json[1];
            this.DownloadedBytes = json[2];
            //// 3 = priority
            this.PieceOffset = json[4];
            this.Pieces = json[5];
            this.Streamable = json[6];
            //// 7 = rate
            this.EtaInSecs = json[8];
            this.HorizontalResolution = json[9];
            this.VerticalResolution = json[10];
        }

        /// <summary>
        /// Gets the path of the file inside the torrent
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the size of the file inside the torrent
        /// </summary>
        public long SizeInBytes { get; private set; }
        
        /// <summary>
        /// Gets the number of bytes downloaded for the file
        /// </summary>
        public long DownloadedBytes { get; private set; }
        
        /// <summary>
        /// Gets the piece offset of the start of the file in the torrent
        /// </summary>
        public long PieceOffset { get; private set; }
        
        /// <summary>
        /// Gets the number of pieces in the file
        /// </summary>
        public long Pieces { get; private set; }
        
        /// <summary>
        /// Gets a value indicating whether the file is streamable or not
        /// </summary>
        public bool Streamable { get; private set; }
        
        /// <summary>
        /// Gets an estimate of the number of seconds left to download the file
        /// </summary>
        public int EtaInSecs { get; private set; }
        
        /// <summary>
        /// Gets the horizontal resolution of the file if available
        /// </summary>
        public int HorizontalResolution { get; private set; }
        
        /// <summary>
        /// Gets the vertical resolution of the file if available
        /// </summary>
        public int VerticalResolution { get; private set; }
    }
}
