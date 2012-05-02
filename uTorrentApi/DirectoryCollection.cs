//-----------------------------------------------------------------------
// <copyright file="DirectoryCollection.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UTorrentAPI.Protocol;

    /// <summary>
    /// A collection of directories that uTorrent can save to.
    /// </summary>
    [Serializable]
    public class DirectoryCollection : IEnumerable<Directory>, IJsonLoadable
    {
        private List<Directory> internalList = new List<Directory>();

        internal DirectoryCollection() 
        {
        }

        /// <summary>
        /// Gets the number of directories that uTorrent can store to
        /// </summary>
        public int Count
        {
            get
            {
                return this.internalList.Count;
            }
        }

        /// <summary>
        /// Gets the Directory object stored at the specified index
        /// </summary>
        /// <param name="i">index of the directory</param>
        /// <returns>a directory where uTorrent can store downloads</returns>
        public Directory this[int i]
        {
            get
            {
                return this.internalList[i];
            }
        }

        /// <summary>
        /// Sets the state of the object based
        /// on the supplied json
        /// </summary>
        /// <param name="json">Json that will be used</param>
        void IJsonLoadable.LoadFromJson(JsonBaseType json)
        {
            JsonArray j = json["root"]["download-dirs"] as JsonArray;
            for (int x = 0; x < j.Count; x++)
            {
                this.internalList.Add(new Directory(j[x]["path"], j[x]["available"]));
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator for the collection</returns>
        public IEnumerator<Directory> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator for the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }
    }
}
