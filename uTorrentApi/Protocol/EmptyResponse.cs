//-----------------------------------------------------------------------
// <copyright file="EmptyResponse.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Basically a "void" return value from the
    /// uTorrent API.  It includes a build number
    /// and nothing else.
    /// </summary>
    [DataContract(Namespace = "")]
    internal class EmptyResponse
    {
        /// <summary>
        /// Gets or sets the build number returned by uTorrent
        /// </summary>
        [DataMember(Name = "build", Order = 1)]
        public int BuildNumber { get; set; }
    }
}
