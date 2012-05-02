//-----------------------------------------------------------------------
// <copyright file="FileCollection.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UTorrentAPI.Protocol;

    /// <summary>
    /// Holds a collection of files that are contained in a torrent
    /// </summary>
    [Serializable]
    public class FileCollection : IEnumerable<File>, IJsonLoadable
    {
        private List<File> internalList = new List<File>();

        /// <summary>
        /// Initializes a new instance of the FileCollection class.  Internal constructor prevents instantiation outside of this assembly
        /// </summary>
        internal FileCollection()
        {
        }

        /// <summary>
        /// Gets the number of files in the torrent
        /// </summary>
        public int Count
        {
            get
            {
                return this.internalList.Count;
            }
        }

        /// <summary>
        /// Gets a File object contained in a torrent at the specified index
        /// </summary>
        /// <param name="i">index of file</param>
        /// <returns>a file in the torrent</returns>
        public File this[int i]
        {
            get
            {
                return this.internalList[i];
            }
        }

        void IJsonLoadable.LoadFromJson(JsonBaseType json)
        {
            JsonArray j = json["root"]["files"][1] as JsonArray;
            for (int x = 0; x < j.Count; x++)
            {
                this.internalList.Add(new File(j[x] as JsonArray));
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection
        /// </summary>
        /// <returns>An enumerator for the collection</returns>
        public IEnumerator<File> GetEnumerator()
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
