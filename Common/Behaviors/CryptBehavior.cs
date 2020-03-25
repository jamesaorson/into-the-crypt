using Common.Enums;
using Komodo.Core.ECS.Components;
using Komodo.Core.ECS.Entities;
using Komodo.Core.Engine.Graphics;
using Komodo.Core.Physics;
using Komodo.Lib.Math;
using System;
using System.Collections.Generic;
using System.Linq;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class CryptBehavior : BehaviorComponent
    {
        #region Constructors
        public CryptBehavior(
            int seed,
            Entity player,
            string ceilingModelPath,
            string floorModelPath,
            string wallModelPath,
            string ceilingTexturePath,
            string floorTexturePath,
            string wallTexturePath,
            uint height = 5,
            uint width = 5,
            uint sectionSize = 4
        ) : base()
        {
            CeilingModelPath = ceilingModelPath;
            CeilingTexturePath = ceilingTexturePath;
            CryptMap = new Dictionary<int, Dictionary<int, Tiles>>();
            FloorModelPath = floorModelPath;
            FloorTexturePath = floorTexturePath;
            Height = Math.Max(20, height);
            Player = player;
            RefreshInterval = 0.10f;
            SectionSize = Math.Max(4, sectionSize);
            Seed = seed;
            WallModelPath = wallModelPath;
            WallTexturePath = wallTexturePath;
            Width = Math.Max(20, width);

            _ceilingTiles = new Dictionary<int, Dictionary<int, Drawable3DComponent>>();
            _floorTiles = new Dictionary<int, Dictionary<int, Drawable3DComponent>>();
            _nearbyColliders = new List<RigidBodyComponent>();
            _walls = new Dictionary<int, Dictionary<int, (Drawable3DComponent, StaticBodyComponent)>>();
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public string CeilingModelPath { get; private set; }
        public string CeilingTexturePath { get; private set; }
        public Model CeilingModel { get; private set; }
        public Texture CeilingTexture { get; private set; }
        public Dictionary<int, Dictionary<int, Tiles>> CryptMap { get; private set; }
        public Vector3 ExitCoordinates { get; private set; }
        public string FloorModelPath { get; private set; }
        public string FloorTexturePath { get; private set; }
        public Model FloorModel { get; private set; }
        public Texture FloorTexture { get; private set; }
        public uint Height { get; private set; }
        public static float HorizontalScaleRatio { get; private set; }
        public Entity Player { get; set; }
        public float RefreshInterval { get; set; }
        public Drawable3DComponent RootCeiling { get; private set; }
        public Drawable3DComponent RootFloor { get; private set; }
        public Drawable3DComponent RootWall { get; private set; }
        public uint SectionSize { get; private set; }
        public int Seed { get; private set; }
        public string WallModelPath { get; private set; }
        public string WallTexturePath { get; private set; }
        public Model WallModel { get; private set; }
        public Texture WallTexture { get; private set; }
        public uint Width { get; private set; }
        #endregion Public Members

        #region Private Members
        private float _accumulatedTime { get; set; }
        private Dictionary<int, Dictionary<int, Drawable3DComponent>> _ceilingTiles { get; set; }
        private List<RigidBodyComponent> _nearbyColliders { get; set; }
        private Dictionary<int, Dictionary<int, Drawable3DComponent>> _floorTiles { get; set; }
        private List<Tiles[,]> _horizontalHallways { get; set; }
        private bool _isNewlyGenerated { get; set; }
        private List<Tiles[,]> _verticalHallways { get; set; }
        private Dictionary<int, Dictionary<int, (Drawable3DComponent, StaticBodyComponent)>> _walls { get; set; }
        #endregion Private Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public void EnableNearbyColliders(Vector3 worldPosition)
        {
            (int x, int z) = ToTilePosition(worldPosition);
            EnableNearbyColliders(x, z);
        }

        public void EnableNearbyColliders(int x, int z)
        {
            foreach (var wall in _nearbyColliders)
            {
                wall.IsEnabled = false;
            }
            _nearbyColliders.Clear();
            for (int i = -5; i < 5; i++)
            {
                for (int j = -5; j < 5; j++)
                {
                    var collider = GetCollider(x + i, z + j);
                    if (collider != null)
                    {
                        collider.IsEnabled = true;
                        _nearbyColliders.Add(collider);
                    }
                }
            }
        }

        public void GenerateCrypt()
        {
            _isNewlyGenerated = true;
            GenerateHallways();
            GenerateCryptMap();
        }

        public RigidBodyComponent GetCollider(Vector3 worldPosition)
        {
            (int x, int z) = ToTilePosition(worldPosition);
            return GetCollider(x, z);
        }

        public RigidBodyComponent GetCollider(int x, int z)
        {
            RigidBodyComponent collider = null;
            if (_walls.ContainsKey(x))
            {
                var row = _walls[x];
                if (row.ContainsKey(z))
                {
                    (_, collider) = row[z];
                }
            }
            return collider;
        }

        public Drawable3DComponent GetTile(Vector3 worldPosition)
        {
            (int x, int z) = ToTilePosition(worldPosition);
            return GetTile(x, z);
        }

        public Tiles GetTileInfo(Vector3 worldPosition)
        {
            (int x, int z) = ToTilePosition(worldPosition);
            return GetTileInfo(x, z);
        }

        public Drawable3DComponent GetTile(int x, int z)
        {
            Drawable3DComponent tile = null;
            if (_walls.ContainsKey(x))
            {
                var row = _walls[x];
                if (row.ContainsKey(z))
                {
                    (tile, _) = row[z];
                }
            }
            return tile;
        }

        public Tiles GetTileInfo(int x, int z)
        {
            var tile = Tiles.Empty;
            if (CryptMap.ContainsKey(x))
            {
                var row = CryptMap[x];
                if (row.ContainsKey(z))
                {
                    tile = row[z];
                }
            }
            return tile;
        }

        public override void Initialize()
        {
            base.Initialize();

            RootCeiling = new Drawable3DComponent(CeilingModelPath)
            {
                TexturePath = CeilingTexturePath
            };
            RootFloor = new Drawable3DComponent(FloorModelPath)
            {
                TexturePath = FloorTexturePath
            };
            RootWall = new Drawable3DComponent(WallModelPath)
            {
                TexturePath = WallTexturePath
            };
            Parent.AddComponent(RootCeiling);
            Parent.AddComponent(RootFloor);
            Parent.AddComponent(RootWall);

            Parent.Position = Vector3.Zero;
            Parent.Scale = new Vector3(
                RootBehavior.TileSize,
                RootBehavior.TileSize,
                RootBehavior.TileSize
            );
        }

        public override void Update(GameTime gameTime)
        {
            if (_isNewlyGenerated)
            {
                _isNewlyGenerated = false;

                RootCeiling.IsEnabled = false;
                RootFloor.IsEnabled = false;
                RootWall.IsEnabled = false;
                var keys = CryptMap.Keys.ToList();
                foreach (var i in keys)
                {
                    var innerKeys = CryptMap[i].Keys.ToList();
                    foreach (var j in innerKeys)
                    {
                        switch (CryptMap[i][j])
                        {
                            case Tiles.Wall:
                                AddWall(i, j);
                                break;
                            case Tiles.Exit:
                                Console.WriteLine("Exit...");
                                CryptMap[i][j] = Tiles.Exit;
                                AddCeiling(i, j);
                                break;
                            default:
                                AddFloor(i, j);
                                AddCeiling(i, j);
                                break;
                        }
                    }
                }
            }
            _accumulatedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_accumulatedTime >= RefreshInterval)
            {
                _accumulatedTime = 0f;
                EnableNearbyColliders(Player.Position);
            }
        }
        #endregion Public Member Methods

        #region Private Member Methods
        private void AddCeiling(int x, int z)
        {
            var ceiling = new Drawable3DComponent(RootCeiling.ModelData)
            {
                Position = ToWorldPosition(x, z) + Vector3.Up * (2 * 2 * RootBehavior.TileSize),
                Texture = RootCeiling.Texture,
            };
            Parent.AddComponent(ceiling);
            if (!_ceilingTiles.ContainsKey(x))
            {
                _ceilingTiles[x] = new Dictionary<int, Drawable3DComponent>();
            }
            _ceilingTiles[x][z] = ceiling;
        }

        private void AddFloor(int x, int z)
        {
           CryptMap[x][z] = Tiles.Floor;
            var floor = new Drawable3DComponent(RootFloor.ModelData)
            {
                Position = ToWorldPosition(x, z),
                Texture = RootFloor.Texture,
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
            var wall = new Drawable3DComponent(RootWall.ModelData)
            {
                Position = wallPosition,
                Texture = RootWall.Texture,
            };
            var wallCollider = new StaticBodyComponent(new Box(2f, 2f, 2f, 1f))
            {
                Position = wallPosition,
                IsEnabled = false,
            };
            Parent.AddComponent(wallCollider);
            Parent.AddComponent(wall);
            if (!_walls.ContainsKey(x))
            {
                _walls[x] = new Dictionary<int, (Drawable3DComponent, StaticBodyComponent)>();
            }
            _walls[x][z] = (wall, wallCollider);
            CryptMap[x][z] = Tiles.Wall;
        }

        private void GenerateCryptMap()
        {
            var random = new Random(Seed);
            for (int i = 0; i < Height; i++)
            {
                CryptMap[i] = new Dictionary<int, Tiles>();
                for (int j = 0; j < Width; j++)
                {
                    CryptMap[i][j] = Tiles.Wall;
                }
            }

            for (int i = 1; i < Height - 1; i += (int)SectionSize - 1)
            {
                for (int j = 1; j < Width - 1; j += (int)SectionSize - 1)
                {
                    Tiles[,] cryptSection;
                    float choice = random.Next(0, 10);
                    if (choice > 5)
                    {
                        cryptSection = _horizontalHallways[random.Next(0, _horizontalHallways.Count)];
                    }
                    else
                    {
                        cryptSection = _verticalHallways[random.Next(0, _verticalHallways.Count)];
                    }
                    SetSection(j, i, cryptSection);
                }
            }

            ExitCoordinates = new Vector3(2, 0, 2);
            CryptMap[(int)ExitCoordinates.X][(int)ExitCoordinates.Z] = Tiles.Exit;
        }

        private void GenerateHallways()
        {
            var random = new Random(Seed);
            int binaryLength = (int)(2 * (SectionSize - 2));
            int numberOfVariations = binaryLength * binaryLength;
            _horizontalHallways = new List<Tiles[,]>();
            _verticalHallways = new List<Tiles[,]>();

            for (int i = 0; i < numberOfVariations; i++)
            {
                _horizontalHallways.Add(new Tiles[SectionSize, SectionSize]);
                var horizontalVariation = _horizontalHallways[i];
                var binaryCombinations = new List<Tiles>();
                int binaryValue = i;
                for (int j = 0; j < binaryLength; j++)
                {
                    switch (binaryValue % 2)
                    {
                        case 1:
                            int choice = random.Next(0, 10);
                            if (choice > 5)
                            {
                                binaryCombinations.Insert(0, Tiles.Floor);
                            }
                            else
                            {
                                binaryCombinations.Insert(0, Tiles.Wall);
                            }
                            break;
                        case 0:
                            binaryCombinations.Insert(0, Tiles.Wall);
                            break;
                    }
                    binaryValue >>= 1;
                }

                int halfBinaryLength = binaryLength / 2;
                for (int row = 0; row < SectionSize; row++)
                {
                    if (row == 0)
                    {
                        horizontalVariation[row, 0] = Tiles.Floor;
                        horizontalVariation[row, SectionSize - 1] = Tiles.Wall;
                        for (int j = 1; j < halfBinaryLength + 1; j++)
                        {
                            horizontalVariation[row, j] = binaryCombinations[j - 1];
                        }
                    }
                    else if (row == SectionSize - 1)
                    {
                        horizontalVariation[row, 0] = Tiles.Wall;
                        horizontalVariation[row, SectionSize - 1] = Tiles.Wall;
                        for (int j = 1; j < halfBinaryLength + 1; j++)
                        {
                            horizontalVariation[row, j] = binaryCombinations[j + halfBinaryLength - 1];
                        }
                    }
                    else
                    {
                        for (int j = 1; j < halfBinaryLength + 1; j++)
                        {
                            horizontalVariation[row, j] = Tiles.Floor;
                        }
                    }
                }

                _verticalHallways.Add(new Tiles[SectionSize, SectionSize]);
                var verticalVariation = _verticalHallways[i];
                for (int row = 0; row < SectionSize; row++)
                {
                    if (row == 0 || row == SectionSize - 1)
                    {
                        for (int j = 0; j < SectionSize; j++)
                        {
                            if (j == 0 || j == SectionSize - 1)
                            {
                                verticalVariation[row, j] = Tiles.Wall;
                            }
                            else
                            {
                                verticalVariation[row, j] = Tiles.Floor;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < SectionSize; j++)
                        {
                            if (j != 0 || j != SectionSize - 1)
                            {
                                verticalVariation[row, j] = Tiles.Floor;
                            }
                        }
                        verticalVariation[row, 0] = binaryCombinations[(row - 1) * 2];
                        verticalVariation[row, SectionSize - 1] = binaryCombinations[((row - 1) * 2) + 1];
                    }
                }
            }
        }

        private void SetSection(int x, int y, Tiles[,] section)
        {
            for (int row = 0; row < SectionSize; row++)
            {
                for (int column = 0; column < SectionSize; column++)
                {
                    var tile = section[row, column];
                    int rowCoordinate = y + row;
                    int columnCoordinate = x + column;
                    if (
                        rowCoordinate == 0
                        || rowCoordinate == Height - 1
                        || columnCoordinate == 0
                        || columnCoordinate == Width - 1
                    )
                    {
                        tile = Tiles.Wall;
                    }
                    if (CryptMap.ContainsKey(rowCoordinate) && CryptMap[rowCoordinate].ContainsKey(columnCoordinate))
                    {
                        CryptMap[rowCoordinate][columnCoordinate] = tile;
                    }
                }
            }
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
