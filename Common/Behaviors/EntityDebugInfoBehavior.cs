using Komodo.Core.ECS.Components;
using Komodo.Core.ECS.Entities;
using Komodo.Core.Engine.Input;
using Komodo.Lib.Math;

using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class ComponentDebugInfoBehavior : BehaviorComponent
    {
        #region Constructors
        public ComponentDebugInfoBehavior(Component component, string prefix = "") : base()
        {
            DebugComponent = component;
            Prefix = prefix;
            RootPosition = Vector3.Zero;
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public Component DebugComponent { get; }
        public TextComponent PositionText { get; private set; }
        public string Prefix { get; set; }
        public InputInfo PreviousToggleDebug { get; set; }
        public Vector3 RootPosition { get; set; }
        public TextComponent ZoomText { get; private set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            PositionText = new TextComponent("fonts/debug_font", Color.DarkSeaGreen, Game.DefaultSpriteShader, "")
            {
                Position = RootPosition
            };
            Parent.AddComponent(PositionText);
            ZoomText = new TextComponent("fonts/debug_font", Color.DarkSeaGreen, Game.DefaultSpriteShader, "")
            {
                Position = RootPosition + new Vector3(0f, -20f, 0f),
            };
            Parent.AddComponent(ZoomText);
        }

        public override void Update(GameTime gameTime)
        {
            var toggleDebug = InputManager.GetInput("toggle_debug");
            if (PreviousToggleDebug.State == InputState.Down && toggleDebug.State == InputState.Up)
            {
                PositionText.IsEnabled = !PositionText.IsEnabled;
                ZoomText.IsEnabled = !ZoomText.IsEnabled;
            }
            var position = DebugComponent.WorldPosition;
            PositionText.Text = $"{(Prefix ?? "")} X: {position.X} Y: {position.Y} Z: {position.Z}";

            if (DebugComponent is CameraComponent camera)
            {
                ZoomText.Text = $"{(Prefix ?? "")} Zoom: {camera.Zoom}";
            }

            PreviousToggleDebug = toggleDebug;
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}