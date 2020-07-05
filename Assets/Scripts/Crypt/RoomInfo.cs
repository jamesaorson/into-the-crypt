using System.Collections.Generic;
using UnityEngine;

namespace IntoTheCrypt.Crypt
{
    public readonly struct RoomInfo
    {
        #region Public

        #region Constructors
        public RoomInfo(string tag = "", IList<IDictionary<string, Vector3>> connections = null)
        {
            Tag = tag;
            Connections = connections ?? new List<IDictionary<string, Vector3>>();;
        }
        #endregion

        #region Members
        public readonly IList<IDictionary<string, Vector3>> Connections;
        public readonly string Tag;
        #endregion

        #endregion
    }
}