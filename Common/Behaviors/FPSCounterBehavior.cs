using Komodo.Core.ECS.Components;
using Komodo.Core.Engine.Input;
using System;

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class FPSCounterBehavior : BehaviorComponent
    {
        #region Members

        #region Public Members
        public TextComponent CounterText { get; private set;  }
        public InputInfo PreviousToggleDebug { get; set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();

            CounterText = new TextComponent("fonts/debug_font", Color.DarkSeaGreen, Game.DefaultSpriteShader, "")
            {
                Position = Komodo.Lib.Math.Vector3.Zero
            };
            Parent.AddComponent(CounterText);
        }
        public override void Update(GameTime gameTime)
        {
            var toggleDebug = InputManager.GetInput("toggle_debug");
            if (PreviousToggleDebug.State == InputState.Down && toggleDebug.State == InputState.Up)
            {
                CounterText.IsEnabled = !CounterText.IsEnabled;
            }

            CounterText.Text = $"{Math.Round(Game.FramesPerSecond)} FPS";
            
            PreviousToggleDebug = toggleDebug;
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}