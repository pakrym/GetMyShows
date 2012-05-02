//-----------------------------------------------------------------------
// <copyright file="IUTorrentProxy.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// The WCF Client interface for communicating with
    /// the uTorrent web API
    /// </summary>
    [ServiceContract]
    internal interface IUTorrentProxy
    {
        /// <summary>
        /// Calls the list action on the uTorrent web API.  This
        /// call includes the cache id parameter so that only
        /// incremental changes are returned from uTorrent.
        /// </summary>
        /// <param name="cid">The cacheid to use for the request.  "0" means no caching.</param>
        /// <returns>A collection of torrents</returns>
        [WeaklyTypedJsonDeserializer]
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?list=1&cid={cid}")]
        JsonObject ListTorrents(string cid);

        /// <summary>
        /// Adds the specified torrent and starts it immediately
        /// </summary>
        /// <param name="torrentUrl">A url to the torrent to start</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=add-url&s={torrentUrl}")]
        EmptyResponse AddTorrentFromUrl(string torrentUrl);

        /// <summary>
        /// Adds the specified torrent and starts it immediately
        /// </summary>
        /// <param name="torrentUrl">A url to the torrent to start</param>
        /// <param name="downloadDir">The index of the download directory from calling <c>ListDirectories</c></param>
        /// <param name="path">The sub path to use under the directory provided in <c>downloadDir</c></param>
        /// <returns>An empty response</returns>
        [OperationContract(Name = "AddTorrentFromUrlWithDirectory")]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=add-url&s={torrentUrl}&download_dir={downloadDir}&path={path}")]
        EmptyResponse AddTorrentFromUrl(string torrentUrl, int downloadDir, string path);

        /// <summary>
        /// Adds the specified torrent file and starts it immediately
        /// </summary>
        /// <param name="torrentFile">A stream of the torrent file.  The stream must be preencoded as formdata/multipart.</param>
        /// <returns>An empty response</returns>
        [FileUploadSerializer]
        [OperationContract]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=add-file")]
        EmptyResponse AddTorrentFromFile(Stream torrentFile);

        /// <summary>
        /// Adds the specified torrent file and starts it immediately
        /// </summary>
        /// <param name="torrentFile">A stream of the torrent file.  The stream must be preencoded as formdata/multipart.</param>
        /// <param name="downloadDir">The index of the download directory from calling <c>ListDirectories</c></param>
        /// <param name="path">The sub path to use under the directory provided in <c>downloadDir</c></param>
        /// <returns>An empty response</returns>
        [FileUploadSerializer]
        [OperationContract(Name = "AddTorrentFromFileWithDirectory")]
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=add-file&download_dir={downloadDir}&path={path}")]
        EmptyResponse AddTorrentFromFile(Stream torrentFile, int downloadDir, string path);

        [WeaklyTypedJsonDeserializer]
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=getfiles&hash={hash}")]
        FileCollection ListFiles(string hash);

        /// <summary>
        /// Starts the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to start</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=start&hash={torrentHash}")]
        EmptyResponse StartTorrent(string torrentHash);

        /// <summary>
        /// Force starts the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to start</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=forcestart&hash={torrentHash}")]
        EmptyResponse ForceStartTorrent(string torrentHash);

        /// <summary>
        /// Stops the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to stop</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=stop&hash={torrentHash}")]
        EmptyResponse StopTorrent(string torrentHash);

        /// <summary>
        /// Pause the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to pause</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=pause&hash={torrentHash}")]
        EmptyResponse PauseTorrent(string torrentHash);

        /// <summary>
        /// Unpause the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to unpause</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=unpause&hash={torrentHash}")]
        EmptyResponse UnpauseTorrent(string torrentHash);

        /// <summary>
        /// Recheck the torrent represented by the supplied hash
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to recheck</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=recheck&hash={torrentHash}")]
        EmptyResponse RecheckTorrent(string torrentHash);

        /// <summary>
        /// Remove the torrent job represented by the supplied hash.  Torrent file and data is left intact.
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to remove</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=remove&hash={torrentHash}")]
        EmptyResponse Remove(string torrentHash);

        /// <summary>
        /// Remove the torrent represented by the supplied hash.  Data is left intact.
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to remove</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=removetorrent&hash={torrentHash}")]
        EmptyResponse RemoveTorrent(string torrentHash);

        /// <summary>
        /// Remove the torrent represented by the supplied hash including data
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent to remove</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=removedatatorrent&hash={torrentHash}")]
        EmptyResponse RemoveTorrentAndData(string torrentHash);

        /// <summary>
        /// Removes the data associated with a torrent
        /// </summary>
        /// <param name="torrentHash">the hash of the torrent whose data should be removed</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=removedata&hash={torrentHash}")]
        EmptyResponse RemoveData(string torrentHash);

        /// <summary>
        /// List the directories that uTorrent can save to
        /// </summary>
        /// <returns>A collection of directories that uTorrent can save to</returns>
        [WeaklyTypedJsonDeserializer]
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=list-dirs")]
        DirectoryCollection ListDirectories();

        /// <summary>
        /// Sets a property on a torrent
        /// </summary>
        /// <param name="torrentHash">The infohash of the torrent to modify</param>
        /// <param name="propertyName">The name of the property to modify</param>
        /// <param name="propertyValue">The new value the property should be set to</param>
        /// <returns>An empty response</returns>
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/?action=setprops&s={propertyName}&hash={torrentHash}&v={propertyValue}")]
        EmptyResponse SetTorrentProperty(string torrentHash, string propertyName, string propertyValue);

        /// <summary>
        /// Gets the current security token for the uTorrent API
        /// </summary>
        /// <returns>The current security token</returns>
        [SecurityTokenProvider(TokenXPath = "/html/div[@id='token']/text()")]
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "/token.html")]
        string GetToken();
    }
}
