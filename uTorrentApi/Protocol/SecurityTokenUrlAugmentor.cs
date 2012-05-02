//-----------------------------------------------------------------------
// <copyright file="SecurityTokenUrlAugmentor.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using System.Text;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Internal type")]
    internal class SecurityTokenUrlAugmentor : IClientMessageInspector
    {
        private readonly string tokenOperation;
        private static readonly DateTime startOfEpoch = new DateTime(1970, 1, 1);
        private static readonly BindingFlags publicInstanceMethod = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
        private string token;
  
        private DateTime tokenLastUpdate = DateTime.MinValue;

        private TimeSpan tokenUpdateInterval = TimeSpan.FromMinutes(20);

        public SecurityTokenUrlAugmentor(string tokenOperation)
        {
            this.tokenOperation = tokenOperation;
        }

        private long TimeStamp
        {
            get { return (long)(DateTime.Now - startOfEpoch).TotalMilliseconds; }
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Do nothing
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (!request.Properties.ContainsKey(SecurityTokenProvider.TokenOperationPropertyName))
            {
                UriBuilder uriWithToken = new UriBuilder((Uri)request.Headers.To);
                if (!string.IsNullOrEmpty(uriWithToken.Query))
                {
                    // Add the token and timestamp to the uri
                    StringBuilder qs = new StringBuilder(100);
                    qs.Append("token=");
                    qs.Append(this.GetToken(channel));
                    qs.Append('&');
                    qs.Append(uriWithToken.Query.Substring(1));
                    qs.Append("&t=");
                    qs.Append(this.TimeStamp);
                    uriWithToken.Query = qs.ToString();
                    request.Headers.To = uriWithToken.Uri;
                }
            }
            else
            {
                request.Properties.Remove(SecurityTokenProvider.TokenOperationPropertyName);
            }

            // AfterReceiveReply has empty implementation, so no correlationState is needed
            return null;
        }

        private string GetToken(IChannel channel)
        {
            if ((DateTime.Now - this.tokenLastUpdate) < this.tokenUpdateInterval)
            {
                return this.token;
            }

            this.token = (string)channel.GetType().InvokeMember(this.tokenOperation, publicInstanceMethod, null, channel, null);
            this.tokenLastUpdate = DateTime.Now;

            return this.token;
        }
    }
}
