//-----------------------------------------------------------------------
// <copyright file="WeaklyTypedJsonDeserializer.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    /// <summary>
    /// Deserializes WCF message and initializes a IJsonLoadable object.
    /// Uses the decorator pattern for wrapping another formatter.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    internal class WeaklyTypedJsonDeserializer : IClientMessageFormatter
    {
        /// <summary>
        /// The return type that will be used for deserialization
        /// </summary>
        private Type returnType;

        /// <summary>
        /// The decorated formatter that will be called during deserialization
        /// </summary>
        private IClientMessageFormatter originalFormatter;

        internal WeaklyTypedJsonDeserializer(IClientMessageFormatter originalFormatter, Type returnType)
        {
            this.originalFormatter = originalFormatter;
            this.returnType = returnType;
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            JsonObject json = new JsonObject(message.GetReaderAtBodyContents());

            if (this.returnType == typeof(JsonObject))
            {
                return json;
            }

            IJsonLoadable returnObject = (IJsonLoadable)Activator.CreateInstance(this.returnType, true);
            returnObject.LoadFromJson(json);
            return returnObject;
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            return this.originalFormatter.SerializeRequest(messageVersion, parameters);
        }
    }
}
