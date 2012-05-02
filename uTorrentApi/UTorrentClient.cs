//-----------------------------------------------------------------------
// <copyright file="UTorrentClient.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using UTorrentAPI.Protocol;

    /// <summary>
    /// This is the main entrypoint to the UTorrentAPI
    /// and provides access to the torrent job list,
    /// program settings, etc.
    /// </summary>
    /// <remarks>All of the objects created by this API have some shared resources
    /// (for example, the underlying channel used to connect to uTorrent).  I have done
    /// my best to allow for threadsafe access across objects in the API, but threadsafety
    /// is not guaranteed.</remarks>
    public sealed class UTorrentClient : IDisposable
    {
        /// <summary>
        /// A WCF channel used to communicate with uTorrent.
        /// </summary>
        private IUTorrentProxy proxy;
        
        /// <summary>
        /// A WCF channel factory used to construct the channel.
        /// </summary>
        private WebChannelFactory<IUTorrentProxy> channelFactory;

        /// <summary>
        /// Initializes a new instance of the UTorrentClient class.
        /// </summary>
        /// <param name="webApiUri">The uri of the uTorrent web api</param>
        /// <param name="username">The username to use to log into uTorrent</param>
        /// <param name="password">The password to use</param>
        /// <param name="maxIncomingMessageSizeInBytes">The size of message to accept from uTorrent web</param>
        public UTorrentClient(Uri webApiUri, string username, string password, long maxIncomingMessageSizeInBytes = 524288)
        {
            CustomBinding clientCustomBinding = new CustomBinding(
                new WebMessageEncodingBindingElement() { ContentTypeMapper = new JsonContentTypeMapper() },
                new HttpTransportBindingElement { UseDefaultWebProxy = true, ManualAddressing = true, AuthenticationScheme = AuthenticationSchemes.Basic, Realm = "uTorrent", AllowCookies = true, MaxReceivedMessageSize = maxIncomingMessageSizeInBytes });

            this.channelFactory = new WebChannelFactory<IUTorrentProxy>(clientCustomBinding, webApiUri);
            this.channelFactory.Credentials.UserName.UserName = username;
            this.channelFactory.Credentials.UserName.Password = password;
            this.proxy = this.channelFactory.CreateChannel();
            this.Torrents = new TorrentCollection(this.proxy);
        }

        /// <summary>
        /// Gets the current collection of torrent jobs.
        /// </summary>
        public TorrentCollection Torrents
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the directories that can be used for storing torrent data.
        /// </summary>
        public DirectoryCollection StorageDirectories
        {
            get
            {
                return this.proxy.ListDirectories();
            }
        }

        /// <summary>
        /// Cleans up this instance and closes the underlying
        /// channel and channel factory.
        /// </summary>
        public void Dispose()
        {
            if (this.proxy != null)
            {
                ((IClientChannel)this.proxy).Close();
                this.proxy = null;
            }

            if (this.channelFactory != null)
            {
                this.channelFactory.Close();
                this.channelFactory = null;
            }
        }
    }
}
