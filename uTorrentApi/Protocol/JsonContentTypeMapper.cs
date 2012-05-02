//-----------------------------------------------------------------------
// <copyright file="JsonContentTypeMapper.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System.ServiceModel.Channels;

    /// <summary>
    /// Used by WCF to map the web service response to a WebContentFormat
    /// </summary>
    internal class JsonContentTypeMapper : WebContentTypeMapper
    {
        /// <summary>
        /// Maps a web service response content type to a WebContentFormat.
        /// </summary>
        /// <param name="contentType">the content type of the web service response</param>
        /// <returns>WebContentFormat.Json if the response is text/plain or text/javascript</returns>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            // Have to match text/plain because uTorrent sends it for json responses :-(
            if (contentType.ToLower() == "text/plain" || contentType == "text/javascript")
            {
                return WebContentFormat.Json;
            }
            else
            {
                return WebContentFormat.Default;
            }
        }
    }
}
