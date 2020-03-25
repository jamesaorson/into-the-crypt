using Common.Enums;
using Komodo.Core.ECS.Components;
using Komodo.Core.ECS.Entities;
using Komodo.Core.Engine.Graphics;
using Komodo.Core.Engine.Input;
using Komodo.Lib.Math;

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class MinimapBehavior : BehaviorComponent
    {
        #region Constructors
        public MinimapBehavior(CryptBehavior crypt) : base()
        {
            Crypt = crypt;
            MapScale = 4;
            MinimapHeight = 50;
            MinimapWidth = 50;
            RefreshInterval = 0.25f;
            _defaultMinimapTextureData = new Color[MinimapHeight, MinimapWidth];
            for (int i = 0; i < MinimapHeight; i++)
            {
                for (int j = 0; j < MinimapWidth; j++)
                {
                    _defaultMinimapTextureData[i, j] = Color.Transparent;
                }
            }
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public CryptBehavior Crypt { get; set; }
        public Entity Player { get; set; }
        public int MapScale { get; set; }
        public SpriteComponent Minimap { get; private set; }
        public int MinimapHeight { get; private set; }
        public int MinimapWidth { get; private set; }
        public Texture MinimapTexture { get; private set; }
        public InputInfo PreviousToggleDebug { get; set; }
        public float RefreshInterval { get; private set; }
        #endregion Public Members

        #region Private Members
        private float _accumulatedTime { get; set; }
        private Color[,] _defaultMinimapTextureData { get; set; }
        #endregion Private Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();
            MinimapTexture = Game.GraphicsManager.CreateTexture(_defaultMinimapTextureData);
            Minimap = new SpriteComponent(MinimapTexture, Game.DefaultSpriteShader)
            {
                Position = new Vector3(
                    Game.GraphicsManager.GraphicsDeviceManager.PreferredBackBufferWidth - MinimapWidth * MapScale * 0.5f,
                    -MinimapHeight * MapScale * 0.5f,
                    0f
                ),
            };
            Parent.AddComponent(Minimap);
        }
        public override void Update(GameTime gameTime)
        {
            var toggleDebug = InputManager.GetInput("toggle_debug");
            if (PreviousToggleDebug.State == InputState.Down && toggleDebug.State == InputState.Up)
            {
                Minimap.IsEnabled = !Minimap.IsEnabled;
            }
            _accumulatedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_accumulatedTime >= RefreshInterval)
            {
                _accumulatedTime = 0f;
                GenerateMinimapTexture();
            }
            PreviousToggleDebug = toggleDebug;
        }
        #endregion Public Member Methods

        #region Private Member Methods
        private void GenerateMinimapTexture()
        {
            if (Crypt == null)
            {
                MinimapTexture = Game.GraphicsManager.CreateTexture(_defaultMinimapTextureData);
            }
            else
            {
                var textureData = new Color[Crypt.Height * MapScale, Crypt.Width * MapScale];
                for (int i = 0; i < Crypt.Height * MapScale; i++)
                {
                    for (int j = 0; j < Crypt.Width * MapScale; j++)
                    {
                        int mapX = j / MapScale;
                        int mapY = i / MapScale;
                        var (playerCryptX, playerCryptY) = CryptBehavior.ToTilePosition(Player.Position);
                        if (playerCryptX == mapX && playerCryptY == mapY)
                        {
                            textureData[i, j] = Color.Blue;
                            continue;
                        }
                        textureData[i, j] = (Crypt.GetTileInfo(mapX, mapY)) switch
                        {
                            Tiles.Wall => Color.Black,
                            Tiles.Exit => Color.Green,
                            _ => Color.Red,
                        };
                    }
                }
                MinimapTexture = Game.GraphicsManager.CreateTexture(textureData);
            }
            Minimap.Texture = MinimapTexture;
        }
        #endregion Private Member Methods

        #endregion Member Methods
    }
}