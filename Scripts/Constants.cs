using Godot;
using IntoTheCrypt.Crypt;
using System.Collections.Generic;

namespace IntoTheCrypt
{
	public static class Constants
	{
		#region Public

		#region Static Constructors
		static Constants()
		{
			Rooms = new Dictionary<string, RoomInfo>
			{
				[VILLAGE_ROOM_TAG] = new RoomInfo(
					tag: VILLAGE_ROOM_TAG,
					connections: new List<IDictionary<string, Vector3>>
					{
						new Dictionary<string, Vector3>
						{
							[TEST_ROOM_1_TAG] = new Vector3(0f, 0f, -12f),
						}
					}
				),
				[TEST_ROOM_1_TAG] = new RoomInfo(
					tag: TEST_ROOM_1_TAG,
					connections: new List<IDictionary<string, Vector3>>
					{
						new Dictionary<string, Vector3>
						{
							[TEST_ROOM_1_TAG] = new Vector3(12f, 0f, 0f)
						},
						new Dictionary<string, Vector3>
						{
							[TEST_ROOM_1_TAG] = new Vector3(-12f, 0f, 0f)
						},
						new Dictionary<string, Vector3>
						{
							[TEST_ROOM_2_TAG] = new Vector3(0f, 0f, 12f)
						},
					}
				),
				[TEST_ROOM_2_TAG] = new RoomInfo(
					tag: TEST_ROOM_2_TAG,
					connections: new List<IDictionary<string, Vector3>>
					{
						new Dictionary<string, Vector3>
						{
							[TEST_ROOM_1_TAG] = new Vector3(0f, 0f, -12f)
						},
					}
				),
			};
			RoomTags = new List<string>
			{
				TEST_ROOM_1_TAG,
				TEST_ROOM_2_TAG,
			};
		}
		#endregion

		#region Constants

		public const float DEXTERITY_TO_SPEED_FACTOR = 1f / 10f;
		public const uint TOXIC_DROP_RATE = 4;
		public const uint TOXIC_INCREASE_LEVEL = 15;

		#region Room Information
		public static readonly Dictionary<string, RoomInfo> Rooms;
		#endregion

		#region Tags
		public const string ENEMY_SPAWN_COLLECTION_TAG = "Enemy Spawn Collection";
		public const string ENEMY_SPAWN_TAG = "Enemy Spawn";
		public const string ENEMY_TAG = "Enemy";
		public const string PLAYER_INTERACTOR_TAG = "Player Interactor";
		public const string PLAYER_TAG = "Player";
		public const string TEST_ROOM_1_TAG = "Test Room 1";
		public const string TEST_ROOM_2_TAG = "Test Room 2";
		public const string VILLAGE_ROOM_TAG = "Village Room";
		public static readonly List<string> RoomTags;
		#endregion

		#region Resource Paths
		public const string ResourceGame = "res://Scenes/Game.tscn";
		public const string ResourceMainMenuUI = "res://Scenes/UI/MainMenu/MainMenu.tscn";
		public const string ResourceSquog = "res://Scenes/Enemies/Squog/Squog.tscn";
		public const string ResourceTestRoom1 = "res://Scenes/Rooms/TestRoom1.tscn";
		public const string ResourceTestRoom2 = "res://Scenes/Rooms/TestRoom2.tscn";
		#endregion

		#endregion

		#endregion
	}
}
