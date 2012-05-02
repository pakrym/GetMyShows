//-----------------------------------------------------------------------
// <copyright file="FileUploadSerializer.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System.IO;
    using System.Net;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Used for encoding an uploaded file as multipart/formdata.  This class is a total hack.  It should be
    /// changed to not rely on the WebGet and to bypass most of the WebHttpBehavior stuff.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    internal class FileUploadSerializer : IClientMessageFormatter
    {
        private const string Boundary = "---------------------------7dc7a1e90288";

        /// <summary>
        /// The decorated formatter that will be called during deserialization
        /// </summary>
        private IClientMessageFormatter originalFormatter;

        internal FileUploadSerializer(IClientMessageFormatter originalFormatter)
        {
            this.originalFormatter = originalFormatter;
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            return this.originalFormatter.DeserializeReply(message, parameters);
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            MemoryStream outStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(outStream);
            writer.WriteLine("--" + Boundary);
            writer.WriteLine("Content-Disposition: form-data; name=\"torrent_file\"; filename=\"foo.torrent\"");
            writer.WriteLine("Content-Type: ");
            writer.WriteLine();
            writer.Flush();
            (parameters[0] as Stream).CopyTo(outStream);
            writer.WriteLine();
            writer.WriteLine("--" + Boundary + "--");
            writer.Flush();

            outStream.Seek(0, SeekOrigin.Begin);
            parameters[0] = outStream;

            Message request = this.originalFormatter.SerializeRequest(messageVersion, parameters);
            
            if (!request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                request.Properties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty();
            }

            HttpRequestMessageProperty http = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            http.Headers.Set(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + Boundary);
            http.Headers.Remove(HttpRequestHeader.Expect);

            return request;
        }
    }
}
