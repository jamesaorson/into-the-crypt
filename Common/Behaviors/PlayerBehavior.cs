using Common.Enums;
using Komodo.Core.ECS.Components;
using Komodo.Core.ECS.Entities;
using Komodo.Core.Engine.Input;
using Komodo.Lib.Math;
using System.Linq;

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class PlayerBehavior : BehaviorComponent
    {
        #region Constructors
        public PlayerBehavior(int playerIndex) : base()
        {
            PlayerIndex = playerIndex;
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public CryptBehavior Crypt { get; set; }
        public Tiles CurrentTileInfo { get; set; }
        public MoveBehavior Move { get; set; }
        public int PlayerIndex { get; }
        public InputInfo PreviousInteract { get; set; }
        public TextComponent Prompt { get; set; }
        public RootBehavior Root { get; set; }
        public Entity UI { get; set; }
        public VillageBehavior Village { get; set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();

            if (PlayerIndex == 0)
            {
                Move = new MoveBehavior(PlayerIndex)
                {
                    Crypt = Crypt,
                    SprintFactor = 2f,
                    Village = Village,
                };
                Parent.AddComponent(Move);
                var graphicsDeviceManager = Game.GraphicsManager.GraphicsDeviceManager;
                Prompt = new TextComponent("fonts/debug_font", Color.DeepPink, Game.DefaultSpriteShader, "")
                {
                    IsCentered = true,
                    Position = new Vector3(
                        graphicsDeviceManager.PreferredBackBufferWidth / 2,
                        -graphicsDeviceManager.PreferredBackBufferHeight / 4,
                        0f
                    ),
                };
                UI.AddComponent(Prompt);
            }
        }
        public override void Update(GameTime gameTime)
        {
            var interact = InputManager.GetInput("interact");
            switch (StateManager.State)
            {
                case GameState.Crypt:
                    CurrentTileInfo = Crypt.GetTileInfo(Parent.Position);
                    switch (CurrentTileInfo)
                    {
                        case Tiles.Exit:
                            Prompt.IsEnabled = true;
                            Prompt.Text = $"Press '{interact.Input}' to exit the crypt";
                            if (PreviousInteract.State == InputState.Down && interact.State == InputState.Up)
                            {
                                Prompt.IsEnabled = false;
                                StateManager.State = GameState.Village;
                                Crypt = null;
                                Root.RefreshVillageState();
                            }
                            break;
                        default:
                            Prompt.IsEnabled = false;
                            break;
                    }
                    break;
                case GameState.Village:
                    Prompt.IsEnabled = Move.IsColliding;
                    if (Prompt.IsEnabled)
                    {
                        Prompt.Text = $"Press '{interact.Input}' to enter the crypt";
                        if (PreviousInteract.State == InputState.Down && interact.State == InputState.Up)
                        {
                            StateManager.State = GameState.Crypt;
                            Root.RefreshCryptState();
                        }
                    }
                    break;
                default:
                    break;
            }
            PreviousInteract = interact;
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}