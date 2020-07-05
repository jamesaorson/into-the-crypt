using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IntoTheCrypt.Crypt
{
    public class CryptController : MonoBehaviour
    {
        #region Public

        #region Members
        public GameObject RoomsRoot;
        public GameObject TestRoom1;
        public GameObject TestRoom2;
        public uint NumberOfRooms;
        #endregion

        #endregion

        #region Private

        #region Members
        private IList<GameObject> _rooms = new List<GameObject>();
        #endregion

        #region Member Methods
        private GameObject MapTagToRoom(string tag)
        {
            switch (tag)
            {
                case Constants.TEST_ROOM_1_TAG:
                    return TestRoom1;
                case Constants.TEST_ROOM_2_TAG:
                    return TestRoom2;
                default:
                    throw new Exception("Invalid room tag for mapping");
            }
        }

        private void SpawnRooms()
        {
            var roomInfo = Constants.Rooms[Constants.VILLAGE_ROOM_TAG];
            var rootPosition = Vector3.zero;
            var instantiatedRooms = 0;
            var random = new System.Random();
            var spawnedPositions = new HashSet<Vector3>();
            spawnedPositions.Add(rootPosition);
            var spawnedRooms = new Queue<(RoomInfo, Vector3)>();
            spawnedRooms.Enqueue((roomInfo, rootPosition));
            while (instantiatedRooms < NumberOfRooms && spawnedRooms.Count > 0)
            {
                (roomInfo, rootPosition) = spawnedRooms.Dequeue();
                foreach (var connection in roomInfo.Connections)
                {
                    var possibleRoomTags = connection.Keys.ToList();
                    var index = random.Next(0, possibleRoomTags.Count);
                    var nextRoomTag = possibleRoomTags[index];
                    var nextRootPosition = connection[nextRoomTag];
                    nextRootPosition = rootPosition + nextRootPosition;
                    if (spawnedPositions.Contains(nextRootPosition))
                    {
                        continue;
                    }
                    var room = Instantiate(MapTagToRoom(nextRoomTag), RoomsRoot.transform);
                    room.transform.position = nextRootPosition;
                    
                    spawnedPositions.Add(nextRootPosition);
                    spawnedRooms.Enqueue((Constants.Rooms[nextRoomTag], nextRootPosition));
                    
                    ++instantiatedRooms;
                    if (instantiatedRooms >= NumberOfRooms)
                    {
                        break;
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {
            SpawnRooms();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {   
        }
        #endregion

        #endregion
    }
}