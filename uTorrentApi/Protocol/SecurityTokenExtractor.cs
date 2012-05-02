// -----------------------------------------------------------------------
// <copyright file="SecurityTokenExtractor.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
// -----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Xml;

    /// <summary>
    /// Extracts a token from an Xml message basef off the supplied XPath expression.
    /// Uses the decorator pattern to decorate the previous message formatter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    internal class SecurityTokenExtractor : IClientMessageFormatter
    {
        /// <summary>
        /// The previous message formatter that is invoked using the decorator pattern
        /// </summary>
        private readonly IClientMessageFormatter originalFormatter;

        /// <summary>
        /// The XPath expression used to extract the security token from xml
        /// </summary>
        private readonly string tokenXPath;

        /// <summary>
        /// Initializes a new instance of the SecurityTokenExtractor class.
        /// </summary>
        /// <param name="originalFormatter">The original message formatter that should be decorated</param>
        /// <param name="tokenXPath">The XPath expression used to extract the security token from xml</param>
        internal SecurityTokenExtractor(IClientMessageFormatter originalFormatter, string tokenXPath)
        {
            this.originalFormatter = originalFormatter;
            this.tokenXPath = tokenXPath;
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(message.GetReaderAtBodyContents());
            return doc.SelectSingleNode(this.tokenXPath).Value;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            Message message = this.originalFormatter.SerializeRequest(messageVersion, parameters);
            message.Properties.Add(SecurityTokenProvider.TokenOperationPropertyName, true);
            return message;
        }
    }
}
