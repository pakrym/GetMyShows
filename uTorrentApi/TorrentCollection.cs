//-----------------------------------------------------------------------
// <copyright file="TorrentCollection.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices;
    using UTorrentAPI.Protocol;
    using IO = System.IO;

    /// <summary>
    /// Contains all the current torrent jobs
    /// </summary>
    [ComVisible(false)]
    public class TorrentCollection : IEnumerable<Torrent>, IJsonLoadable
    {
        /// <summary>
        /// Provides actual storage for the torrent objects
        /// </summary>
        private InternalTorrentCollection internalCollection = new InternalTorrentCollection();

        /// <summary>
        /// A reference to the service proxy used to communicate with the
        /// uTorrent web API
        /// </summary>
        private IUTorrentProxy proxy;

        /// <summary>
        /// Keeps track of the cache id of the torrent collection so we can
        /// do incremental updates
        /// </summary>
        private string cid = "0";

        /// <summary>
        /// Initializes a new instance of the TorrentCollection class
        /// </summary>
        /// <param name="proxy">the procol client that this torrent collection should use for updates, etc</param>
        internal TorrentCollection(IUTorrentProxy proxy)
        {
            this.proxy = proxy;
        }

        /// <summary>
        /// Gets the number of torrents currently tracked by uTorrent
        /// </summary>
        public int Count
        {
            get
            {
                this.Update();
                return this.internalCollection.Count;
            }
        }

        /// <summary>
        /// Gets the torrent at the specified index
        /// </summary>
        /// <param name="i">index of the torrent</param>
        /// <returns>torrent at the specified index</returns>
        public Torrent this[int i]
        {
            get
            {
                this.Update();
                return this.internalCollection[i];
            }
        }

        /// <summary>
        /// Gets the torrent with the specified hash
        /// </summary>
        /// <param name="hash">hash of the torrent</param>
        /// <returns>torrent with the specified hash</returns>
        public Torrent this[string hash]
        {
            get
            {
                this.Update();
                return this.internalCollection[hash];
            }
        }

        /// <summary>
        /// Adds and starts a torrent job based off the supplied url and saves data in the supplied path
        /// </summary>
        /// <param name="url">A url to the torrent file</param>
        /// <param name="savePath">The directory where torrent data should be saved</param>
        /// <exception cref="IO.DirectoryNotFoundException">throws if uTorrent is not configured to be able to save to the supplied path</exception>
        public void AddUrl(string url, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
            {
                this.proxy.AddTorrentFromUrl(url);
            }
            else
            {
                int storageDirectory;
                string subDirectory;
                this.ResolveStorageDirectory(savePath, out storageDirectory, out subDirectory);
                this.proxy.AddTorrentFromUrl(url, storageDirectory, subDirectory);
            }
        }

        /// <summary>
        /// Adds the specified torrent file to uTorrent
        /// </summary>
        /// <param name="path">path to the torrent file to add</param>
        /// <param name="savePath">path to save the downloaded torrent to</param>
        public void AddFile(string path, string savePath = null)
        {
            using (IO.Stream stream = IO.File.OpenRead(path))
            {
                this.AddFile(stream, savePath);
            }
        }

        /// <summary>
        /// Adds the specified torrent file to uTorrent
        /// </summary>
        /// <param name="fileContents">a stream containing the contents of a torrent file</param>
        /// <param name="savePath">path to save the downloaded torrent to</param>
        public void AddFile(IO.Stream fileContents, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
            {
                this.proxy.AddTorrentFromFile(fileContents);
            }
            else
            {
                int storageDir;
                string subDir;
                this.ResolveStorageDirectory(savePath, out storageDir, out subDir);
                this.proxy.AddTorrentFromFile(fileContents, storageDir, subDir);
            }
        }

        /// <summary>
        /// Removes all of the finished torrents from uTorrent
        /// </summary>
        /// <param name="removalOptions">the removal options to use</param>
        public void RemoveFinished(TorrentRemovalOptions removalOptions = TorrentRemovalOptions.TorrentFile)
        {
            for (int x = 0; x < this.internalCollection.Count;)
            {
                Torrent t = this.internalCollection[x];
                if (t.Status == TorrentStatus.FinishedOrStopped && t.ProgressInMils == 1000)
                {
                    this.Remove(t, removalOptions);
                }
                else
                {
                    x++;
                }
            }
        }

        /// <summary>
        /// Removes the specified torrent from uTorrent
        /// </summary>
        /// <param name="torrent">the torrent to remove</param>
        /// <param name="removalOptions">the removal options to use</param>
        /// <returns>value is unused--it is always true</returns>
        public bool Remove(Torrent torrent, TorrentRemovalOptions removalOptions = TorrentRemovalOptions.TorrentFile)
        {
            this.Remove(torrent.Hash, removalOptions);
            return true;
        }

        /// <summary>
        /// Removes the specified torrent from uTorrent
        /// </summary>
        /// <param name="torrentHash">the torrent to remove</param>
        /// <param name="removalOptions">the removal options to use</param>
        public void Remove(string torrentHash, TorrentRemovalOptions removalOptions = TorrentRemovalOptions.TorrentFile)
        {
            switch (removalOptions)
            {
                case TorrentRemovalOptions.Job:
                    this.proxy.Remove(torrentHash);
                    break;
                case TorrentRemovalOptions.Data:
                    this.proxy.RemoveData(torrentHash);
                    break;
                case TorrentRemovalOptions.TorrentFile:
                    this.proxy.RemoveTorrent(torrentHash);
                    break;
                case TorrentRemovalOptions.TorrentFileAndData:
                    this.proxy.RemoveTorrentAndData(torrentHash);
                    break;
                default:
                    throw new InvalidOperationException("Invalid removalOptions supplied.");
            }
        }

        /// <summary>
        /// Removes the torrent at the specified index from uTorrent (torrent file only).
        /// </summary>
        /// <param name="index">the index of the torrent to remove</param>
        /// <param name="removalOptions">the removal options to use</param>
        public void RemoveAt(int index, TorrentRemovalOptions removalOptions = TorrentRemovalOptions.TorrentFile)
        {
            this.Remove(this.internalCollection[index].Hash, removalOptions);
        }

        /// <summary>
        /// Causes the in-memory collection to be updated from uTorrent
        /// </summary>
        public void Update()
        {
            JsonObject json = this.proxy.ListTorrents(this.cid);
            (this as IJsonLoadable).LoadFromJson(json);

            // Save the newest cacheId
            this.cid = json["root"]["torrentc"];
        }

        /// <summary>
        /// Determines whether the collection contains a torrent with the given hash
        /// </summary>
        /// <param name="hash">The infohash of a torrent</param>
        /// <returns>True if the torrent is loaded in uTorrent</returns>
        public bool Contains(string hash)
        {
            this.Update();
            return this.internalCollection.Contains(hash);
        }

        /// <summary>
        /// Determines whether the collection contains a given torrent
        /// </summary>
        /// <param name="item">A torrent object</param>
        /// <returns>True if the torrent is loaded in uTorrent</returns>
        public bool Contains(Torrent item)
        {
            this.Update();
            return this.internalCollection.Contains(item.Hash);
        }

        /// <summary>
        /// Returns an enumerator of torrents loaded in uTorrent
        /// </summary>
        /// <returns>An enumerator of torrents</returns>
        public IEnumerator<Torrent> GetEnumerator()
        {
            this.Update();
            return this.internalCollection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator of torrents loaded in uTorrent
        /// </summary>
        /// <returns>An enumerator of torrents</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.Update();
            return this.internalCollection.GetEnumerator();
        }

        /// <summary>
        /// Updates the in-memory collection from its json representation
        /// </summary>
        /// <param name="json">the json to load</param>
        void IJsonLoadable.LoadFromJson(JsonBaseType json)
        {
            JsonArray torrents = json["root"]["torrents"] as JsonArray;

            if (torrents != null)
            {
                this.internalCollection.Clear();

                for (int x = 0; x < torrents.Count; x++)
                {
                    this.internalCollection.Add(new Torrent(torrents[x] as JsonArray, this.proxy));
                }
            }
            else
            {
                torrents = json["root"]["torrentp"] as JsonArray;

                for (int x = 0; x < torrents.Count; x++)
                {
                    JsonArray jsonTorrent = torrents[x] as JsonArray;
                    string hash = jsonTorrent[0];
                    if (this.internalCollection.Contains(hash))
                    {
                        Torrent torrent = this.internalCollection[hash];
                        (torrent as IJsonLoadable).LoadFromJson(jsonTorrent);
                    }
                    else
                    {
                        this.internalCollection.Add(new Torrent(jsonTorrent, this.proxy));
                    }
                }

                torrents = json["root"]["torrentm"] as JsonArray;
                for (int x = 0; x < torrents.Count; x++)
                {
                    string hash = torrents[x];
                    this.internalCollection.Remove(hash);
                }
            }
        }

        private void ResolveStorageDirectory(string savePath, out int storageDirectory, out string subDirectory)
        {
            DirectoryCollection directories = this.proxy.ListDirectories();

            storageDirectory = -1;
            for (int x = 0; x < directories.Count; x++)
            {
                if (savePath.StartsWith(directories[x].Path, StringComparison.OrdinalIgnoreCase))
                {
                    storageDirectory = x;
                    break;
                }
            }

            if (storageDirectory < 0)
            {
                throw new IO.DirectoryNotFoundException(string.Format("uTorrent is not configured to allow saving to the supplied directory: {0}", savePath));
            }

            subDirectory = savePath.Substring(directories[storageDirectory].Path.Length);
        }

        private class InternalTorrentCollection : KeyedCollection<string, Torrent>
        {
            public InternalTorrentCollection() : base(StringComparer.InvariantCultureIgnoreCase)
            {
            }

            /// <summary>
            /// Retrieves the key to use from a torrent object
            /// </summary>
            /// <param name="item">the torrent object from which to extract a key</param>
            /// <returns>the key of the supplied torrent object</returns>
            protected override string GetKeyForItem(Torrent item)
            {
                return item.Hash;
            }
        }
    }
}
