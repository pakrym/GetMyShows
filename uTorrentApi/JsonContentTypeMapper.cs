
namespace UTorrentAPI.Service
{
    using System.ServiceModel.Channels;

    internal class JsonContentTypeMapper : WebContentTypeMapper
    {
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
