using System.Diagnostics.CodeAnalysis;
using Komodo.Core.ECS.Components;
using Komodo.Core.Engine.Input;
using Komodo.Lib.Math;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class CameraBehavior : BehaviorComponent
    {
        #region Constructors
        public CameraBehavior([NotNull] CameraComponent camera) : base()
        {
            Camera = camera;
            PanVelocity = 50f;
            RotationVelocity = 2f;
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public CameraComponent Camera { get; set; }
        public float PanVelocity { get; set; }
        public float RotationVelocity { get; set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Update(GameTime gameTime)
        {
            var cameraZoomIn = InputManager.GetInput("camera_zoom_in", 0);
            var cameraZoomOut = InputManager.GetInput("camera_zoom_out", 0);
            var cameraLeft = InputManager.GetInput("camera_left", 0);
            var cameraRight = InputManager.GetInput("camera_right", 0);

            var rotation = Vector3.Zero;
            float zoomChange = 0f;
            if (cameraZoomIn.State == InputState.Down)
            {
                zoomChange += 1f;
            }
            if (cameraZoomOut.State == InputState.Down)
            {
                zoomChange -= 1f;
            }
            if (cameraLeft.State == InputState.Down)
            {
                rotation += new Vector3(0f, 1f, 0f);
            }
            if (cameraRight.State == InputState.Down)
            {
                rotation -= new Vector3(0f, 1f, 0f);
            }
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Camera.ZoomIn(zoomChange * delta);

            Parent.Rotation += (
                rotation
                * RotationVelocity
                * delta
            );
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}