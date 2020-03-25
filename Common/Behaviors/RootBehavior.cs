using Common.Enums;
using Komodo.Core.ECS.Components;
using Komodo.Core.ECS.Entities;
using Komodo.Core.ECS.Systems;
using Komodo.Lib.Math;
using System.Collections.Generic;
using System.Linq;
using System;

using GameTime = Microsoft.Xna.Framework.GameTime;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace Common.Behaviors
{
    public class RootBehavior : BehaviorComponent
    {
        #region Constructors
        public RootBehavior()
        {
            Enemies = new List<Entity>();
            Random = new Random(9001);
            TileSize = 32;
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public Entity DebugInfo { get; set; }
        public List<Entity> Enemies { get; set; }
        public Entity Crypt { get; set; }
        public CryptBehavior CryptBehavior { get; set; }
        public CameraComponent OrthographicCamera { get; set; }
        public CameraComponent PerspectiveCamera { get; set; }
        public Entity Player { get; set; }
        public Random Random { get; set; }
        public Entity UI { get; set; }
        public MinimapBehavior Minimap { get; set; }
        public Entity Village { get; set; }
        #endregion Public Members

        #endregion Members

        #region Static Members

        #region Public Static Members
        public static uint TileSize { get; set; }
        #endregion Public Static Members

        #endregion Static Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();

            Game.Title = "Into The Crypt";
            Game.GraphicsManager.SetFullscreen(true);
            Game.GraphicsManager.SetResolution(Game.GraphicsManager.Resolutions.Last());
            Game.GraphicsManager.VSync = true;

            StateManager.State = GameState.Village;

            var physics = Game.CreatePhysicsSystem();
            var render2D = Game.CreateRender2DSystem();
            var render3D = Game.CreateRender3DSystem();
            var uiRender2D = Game.CreateRender2DSystem();
            SetupOrthographicCamera(uiRender2D);
            SetupUI(uiRender2D);
            
            SetupPlayer(render2D, render3D, physics, UI);
            SetupPerspectiveCamera(Player);
            RefreshVillageState();
            
            var playerBehavior = Player?.Components?.Where(x => x is PlayerBehavior).FirstOrDefault() as PlayerBehavior;
            SetupDebugInfo(uiRender2D, playerBehavior);
        }

        public void RefreshCryptState()
        {
            if (Crypt != null)
            {
                Crypt.IsEnabled = true;
            }
            if (Village != null)
            {
                Village.IsEnabled = false;
            }
            var playerBehavior = Player?.Components?.Where(x => x is PlayerBehavior).FirstOrDefault() as PlayerBehavior;
            SetupEnemies(Player.Render2DSystem, playerBehavior);
            SetupCrypt(Player.Render3DSystem, Player.PhysicsSystem, playerBehavior);
            CryptBehavior = Crypt?.Components?.Where(x => x is CryptBehavior).FirstOrDefault() as CryptBehavior;
            CryptBehavior.GenerateCrypt();
            if (Player?.Components?.Where(x => x is MoveBehavior).FirstOrDefault() is MoveBehavior moveBehavior)
            {
                moveBehavior.Crypt = CryptBehavior;
            }
            Minimap.Crypt = CryptBehavior;

            var exitWorldPosition = CryptBehavior.ToWorldPosition(
                (int)CryptBehavior.ExitCoordinates.X,
                (int)CryptBehavior.ExitCoordinates.Z
            );
            Player.Position = exitWorldPosition + Vector3.Up * TileSize * 2f;
            //Player.Rotation = new Vector3(0f, MathHelper.ToRadians(270f), 0f);
        }

        public void RefreshVillageState()
        {
            if (Crypt != null)
            {
                Crypt.IsEnabled = false;
            }
            if (Village != null)
            {
                Village.IsEnabled = true;
            }
            var playerBehavior = Player?.Components?.Where(x => x is PlayerBehavior).FirstOrDefault() as PlayerBehavior;
            SetupVillage(Player.Render3DSystem, Player.PhysicsSystem, playerBehavior);
            var villageBehavior = Village?.Components?.Where(x => x is VillageBehavior).FirstOrDefault() as VillageBehavior;
            if (Player?.Components?.Where(x => x is MoveBehavior).FirstOrDefault() is MoveBehavior moveBehavior)
            {
                moveBehavior.Village = villageBehavior;
            }
            var startWorldPosition = CryptBehavior.ToWorldPosition(2, 2);
            Player.Position = startWorldPosition + Vector3.Up * TileSize * 2;
            //Player.Rotation = new Vector3(0f, MathHelper.ToRadians(225f), 0f);
        }

        public override void Update(GameTime gameTime)
        {
            switch (StateManager.State)
            {
                case GameState.Crypt:
                    if (Village != null)
                    {
                        foreach (var component in Village.Components)
                        {
                            component.IsEnabled = false;
                        }
                        Village.ClearComponents();
                        Village = null;
                    }

                    break;
                case GameState.Village:
                    if (Crypt != null)
                    {
                        foreach (var component in Crypt.Components)
                        {
                            component.IsEnabled = false;
                        }
                        Crypt.ClearComponents();
                        Crypt = null;
                        CryptBehavior = null;
                        Minimap.Crypt = null;
                    }
                    foreach (var enemy in Enemies)
                    {
                        foreach (var component in enemy.Components)
                        {
                            component.IsEnabled = false;
                        }
                        enemy.ClearComponents();
                    }
                    Enemies.Clear();
                    break;
                default:
                    break;
            }
        }
        #endregion Public Member Methods

        #region Private Member Methods
        private void SetupCrypt(Render3DSystem render3D, PhysicsSystem physics, PlayerBehavior playerBehavior)
        {
            Crypt = new Entity(Game)
            {
                PhysicsSystem = physics,
                Position = new Vector3(0f, 0f, 0f),
                Render3DSystem = render3D,
            };
            var cryptBehavior = new CryptBehavior(
                seed: Random.Next(),
                player: Player,
                ceilingModelPath: "models/cube",
                floorModelPath: "models/cube",
                wallModelPath: "models/cube",
                ceilingTexturePath: "items/stone",
                floorTexturePath: "items/dirt",
                wallTexturePath: "items/bricks/clay_bricks",
                height: 40,
                width: 40,
                sectionSize: 4
            );
            Crypt.AddComponent(cryptBehavior);
            playerBehavior.Crypt = cryptBehavior;
        }
        private void SetupDebugInfo(Render2DSystem render2D, PlayerBehavior playerBehavior)
        {
            DebugInfo = new Entity(Game)
            {
                Render2DSystem = render2D,
            };
            DebugInfo.AddComponent(new FPSCounterBehavior());
            DebugInfo.AddComponent(
                new ComponentDebugInfoBehavior(playerBehavior, prefix: "Player -")
                {
                    RootPosition = new Vector3(0f, -20f, 0f),
                }
            );
            DebugInfo.AddComponent(
                new ComponentDebugInfoBehavior(PerspectiveCamera, prefix: "Camera -")
                {
                    RootPosition = new Vector3(0f, -40f, 0f),
                }
            );
        }

        private void SetupEnemies(Render2DSystem render2D, PlayerBehavior playerBehavior)
        {
            Enemies = new List<Entity>();
            var enemy = new Entity(Game)
            {
                Render2DSystem = render2D,
            };
            enemy.AddComponent(
                new EnemyBehavior
                {
                    Player = playerBehavior,
                }
            );
            Enemies.Add(enemy);
        }

        private void SetupOrthographicCamera(Render2DSystem render2D)
        {
            var orthographicCameraEntity = new Entity(Game)
            {
                Position = new Vector3(0f, 0f, 0f),
                Render2DSystem = render2D,
            };
            OrthographicCamera = new CameraComponent()
            {
                FarPlane = 10000000f,
                IsPerspective = false,
                Position = new Vector3(0f, 0f, 100f),
            };
            orthographicCameraEntity.AddComponent(OrthographicCamera);
            OrthographicCamera.SetActive();
        }

        private void SetupPerspectiveCamera(Entity root)
        {
            PerspectiveCamera = new CameraComponent()
            {
                Position = new Vector3(0, 15f, 0f),
                FarPlane = 10000000f,
                IsPerspective = true,
                Zoom = 1f,
                MinimumZoom = 1f,
            };
            root.AddComponent(PerspectiveCamera);
            var cameraBehavior = new CameraBehavior(PerspectiveCamera);
            root.AddComponent(cameraBehavior);
            PerspectiveCamera.SetActive();
        }

        private void SetupPlayer(Render2DSystem render2D, Render3DSystem render3D, PhysicsSystem physics, Entity ui)
        {
            Player = new Entity(Game)
            {
                PhysicsSystem = physics,
                Render2DSystem = render2D,
                Render3DSystem = render3D,
            };
            var playerBehavior = new PlayerBehavior(0)
            {
                Root = this,
            };
            Player.AddComponent(playerBehavior);
            playerBehavior.UI = ui;
            Minimap.Player = Player;
        }

        private void SetupUI(Render2DSystem render2D)
        {
            var crypt = Crypt?.Components?.Where(x => x is CryptBehavior).SingleOrDefault() as CryptBehavior;
            UI = new Entity(Game)
            {
                Render2DSystem = render2D
            };

            Minimap = new MinimapBehavior(crypt);
            UI.AddComponent(Minimap);
        }

        private void SetupVillage(Render3DSystem render3D, PhysicsSystem physics, PlayerBehavior playerBehavior)
        {
            Village = new Entity(Game)
            {
                PhysicsSystem = physics,
                Position = Vector3.Zero,
                Render3DSystem = render3D,
            };
            var villageBehavior = new VillageBehavior(
                buildingModelPath: "models/cube",
                buildingTexturePath: "items/furnace/furnace",
                floorModelPath: "models/cube",
                floorTexturePath: "items/dirt",
                wallModelPath: "models/cube",
                wallTexturePath: "items/sand"
            );
            Village.AddComponent(villageBehavior);
            playerBehavior.Village = villageBehavior;
        }
        #endregion Private Member Methods

        #endregion Member Methods
    }
}