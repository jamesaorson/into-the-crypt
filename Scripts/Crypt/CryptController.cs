using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using static Godot.Input;

namespace IntoTheCrypt.Crypt
{
	public class CryptController : Spatial
	{
		#region Public

		#region Members
		public Spatial RoomsRoot { get; private set; }
		[Export(PropertyHint.Range, "0,20,or_greater")]
		public uint NumberOfRooms;
		#endregion
		
		#region Public
		
		#region Member Methods
		public override void _Ready()
		{
			GD.Print("Loaded in to crypt...");
			GetNodeReferences();
			LoadScenes();
			SpawnRooms();
			Input.SetMouseMode(MouseMode.Captured);
		}
		#endregion
		
		#endregion

		#endregion

		#region Private

		#region Members
		private IList<Spatial> _rooms = new List<Spatial>();
		private PackedScene _mainMenuScene;
		private PackedScene _testRoom1;
		private PackedScene _testRoom2;
		#endregion

		#region Member Methods
		private void GetNodeReferences()
		{
			RoomsRoot = GetNode<Spatial>("Rooms");
		}

		private Spatial InstantiateRoom(string roomTag)
		{
			var room = MapTagToRoom(roomTag).Instance() as Spatial;
			if (room == null)
			{
				throw new Exception($"Expected {roomTag} to be Spatial");
			}
			return room;
		}

		private void LoadScenes()
		{
			_testRoom1 = GD.Load<PackedScene>(Constants.ResourceTestRoom1);
			if (_testRoom1 == null)
			{
				throw new Exception("TestRoom1 scene did not load correctly");
			}
			_testRoom2 = GD.Load<PackedScene>(Constants.ResourceTestRoom2);
			if (_testRoom2 == null)
			{
				throw new Exception("TestRoom2 scene did not load correctly");
			}
		}

		private PackedScene MapTagToRoom(string tag)
		{
			switch (tag)
			{
				case Constants.TEST_ROOM_1_TAG:
					return _testRoom1;
				case Constants.TEST_ROOM_2_TAG:
					return _testRoom2;
				default:
					throw new Exception("Invalid room tag for mapping");
			}
		}

		private void SpawnRooms()
		{
			GD.Print("Spawning rooms...");
			var roomInfo = Constants.Rooms[Constants.VILLAGE_ROOM_TAG];
			var rootPosition = Vector3.Zero;
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
					var room = InstantiateRoom(nextRoomTag);
					room.Translation = nextRootPosition;
					RoomsRoot.AddChild(room);
					
					spawnedPositions.Add(nextRootPosition);
					spawnedRooms.Enqueue((Constants.Rooms[nextRoomTag], nextRootPosition));
					
					++instantiatedRooms;
					if (instantiatedRooms >= NumberOfRooms)
					{
						break;
					}
				}
			}
			GD.Print("Done spawning rooms!");
		}
		#endregion

		#endregion
	}
}
