using Komodo.Core.ECS.Components;
using Komodo.Core.Engine.Input;
using Komodo.Core.Physics;
using Komodo.Lib.Math;
using System;
using System.Collections.Generic;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class VillageBehavior : BehaviorComponent
    {
        #region Constructors
        public VillageBehavior(
            string buildingModelPath,
            string buildingTexturePath,
            string floorModelPath,
            string floorTexturePath,
            string wallModelPath,
            string wallTexturePath
        ) : base()
        {
            BuildingModelPath = buildingModelPath;
            BuildingTexturePath = buildingTexturePath;
            FloorModelPath = floorModelPath;
            FloorTexturePath = floorTexturePath;
            WallModelPath = wallModelPath;
            WallTexturePath = wallTexturePath;

            _buildings = new Dictionary<int, Dictionary<int, Drawable3DComponent>>();
            _floorTiles = new Dictionary<int, Dictionary<int, Drawable3DComponent>>();
            _walls = new Dictionary<int, Dictionary<int, Drawable3DComponent>>();
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public string BuildingModelPath { get; private set; }
        public string BuildingTexturePath { get; private set; }
        public string FloorModelPath { get; private set; }
        public string FloorTexturePath { get; private set; }
        public InputInfo PreviousToggleDebug { get; set; }
        public string WallModelPath { get; private set; }
        public string WallTexturePath { get; private set; }
        #endregion Public Members

        #region Private Members }
        private Dictionary<int, Dictionary<int, Drawable3DComponent>> _buildings { get; set; }
        private Dictionary<int, Dictionary<int, Drawable3DComponent>> _floorTiles { get; set; }
        private Dictionary<int, Dictionary<int, Drawable3DComponent>> _walls { get; set; }
        #endregion Private Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public Drawable3DComponent GetTile(Vector3 worldPosition)
        {
            (int x, int z) = ToTilePosition(worldPosition);
            return GetTile(x, z);
        }

        public Drawable3DComponent GetTile(int x, int z)
        {
            Drawable3DComponent tile = null;
            if (_buildings.ContainsKey(x))
            {
                var row = _buildings[x];
                if (row.ContainsKey(z))
                {
                    tile = row[z];
                }
            }
            if (tile == null)
            {
                if (_walls.ContainsKey(x))
                {
                    var row = _walls[x];
                    if (row.ContainsKey(z))
                    {
                        tile = row[z];
                    }
                }
            }
            return tile;
        }

        public override void Initialize()
        {
            base.Initialize();

            int height = 10;
            int width = 10;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    AddFloor(i, j);

                    if (i == height / 2 && j == width / 2)
                    {
                        AddBuilding(i, j);
                    }

                    if (
                        i == 0
                        || i == height - 1
                        || j == 0
                        || j == width - 1
                    )
                    {
                        AddWall(i, j);
                    }
                }
            }

            Parent.Position = Vector3.Zero;
            Parent.Scale = new Vector3(RootBehavior.TileSize, RootBehavior.TileSize, RootBehavior.TileSize);

            //IsEnabled = false;
        }

        public override void Update(GameTime gameTime)
        {
        }
        #endregion Public Member Methods

        #region Private Member Methods
        private void AddBuilding(int x, int z)
        {
            var buildingPosition = ToWorldPosition(x, z) + Vector3.Up * (2 * RootBehavior.TileSize);
            var building = new Drawable3DComponent(BuildingModelPath)
            {
                Position = buildingPosition,
                TexturePath = BuildingTexturePath,
            };
            var buildingCollider = new StaticBodyComponent(new Box(2f, 1f, 2f, 1f))
            {
                Position = buildingPosition,
            };
            Parent.AddComponent(building);
            Parent.AddComponent(buildingCollider);
            if (!_buildings.ContainsKey(x))
            {
                _buildings[x] = new Dictionary<int, Drawable3DComponent>();
            }
            _buildings[x][z] = building;
        }

        private void AddFloor(int x, int z)
        {
            var floor = new Drawable3DComponent(FloorModelPath)
            {
                Position = ToWorldPosition(x, z),
                TexturePath = FloorTexturePath,
            };
            Parent.AddComponent(floor);
            if (!_floorTiles.ContainsKey(x))
            {
                _floorTiles[x] = new Dictionary<int, Drawable3DComponent>();
            }
            _floorTiles[x][z] = floor;
        }

        private void AddWall(int x, int z)
        {
            var wallPosition = ToWorldPosition(x, z) + Vector3.Up * (2 * RootBehavior.TileSize);
            var wall = new Drawable3DComponent(WallModelPath)
            {
                Position = wallPosition,
                TexturePath = WallTexturePath,
            };
            var wallCollider = new StaticBodyComponent(new Box(2f, 2f, 2f, 1f))
            {
                Position = wallPosition,
            };
            Parent.AddComponent(wallCollider);
            Parent.AddComponent(wall);
            if (!_walls.ContainsKey(x))
            {
                _walls[x] = new Dictionary<int, Drawable3DComponent>();
            }
            _walls[x][z] = wall;
        }
        #endregion Private Member Methods

        #endregion Member Methods

        #region Static Methods

        #region Public Static Methods
        public static (int, int) ToTilePosition(Vector3 worldPosition)
        {
            worldPosition = worldPosition / RootBehavior.TileSize / 2;
            int x = (int)Math.Round(worldPosition.X);
            int z = (int)Math.Round(worldPosition.Z);
            return (x, z);
        }

        public static Vector3 ToWorldPosition(int tileX, int tileZ)
        {
            return new Vector3(tileX, 0f, tileZ) * RootBehavior.TileSize * 2;
        }
        #endregion Public Static Methods

        #endregion Static Methods
    }
}
