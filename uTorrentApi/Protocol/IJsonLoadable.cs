//-----------------------------------------------------------------------
// <copyright file="IJsonLoadable.cs" company="Mike Davis">
//     To the extent possible under law, Mike Davis has waived all copyright and related or neighboring rights to this work.  This work is published from: United States.  See copying.txt for details.  I would appreciate credit when incorporating this work into other works.  However, you are under no legal obligation to do so.
// </copyright>
//-----------------------------------------------------------------------

namespace UTorrentAPI.Protocol
{
    /// <summary>
    /// Objects that can be initialized from JsonObject.
    /// </summary>
    internal interface IJsonLoadable
    {
        /// <summary>
        /// Sets the state of the object based
        /// on the supplied json
        /// </summary>
        /// <param name="json">Json that will be used</param>
        void LoadFromJson(JsonBaseType json);
    }
}
