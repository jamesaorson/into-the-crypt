using Common.Enums;
using Komodo.Core.ECS.Components;
using Komodo.Core.Engine.Input;
using Komodo.Core.Physics;
using Komodo.Lib.Math;
using System.Linq;
using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Common.Behaviors
{
    public class MoveBehavior : BehaviorComponent
    {
        #region Constructors
        public MoveBehavior(int playerIndex) : base()
        {
            if (!InputManager.IsValidPlayerIndex(playerIndex))
            {
                playerIndex = 0;
            }
            PlayerIndex = playerIndex;
            RotationVelocity = 2f;
            SprintFactor = 2f;
            Velocity = 50f;
        }
        #endregion Constructors

        #region Members

        #region Public Members
        public KinematicBodyComponent Body { get; private set; }
        public CameraComponent Camera => Parent?.Render3DSystem?.ActiveCamera;
        public CryptBehavior Crypt { get; set; }
        public bool IsColliding { get; private set; }
        public int PlayerIndex { get; private set; }
        public float RotationVelocity { get; set; }
        public float SprintFactor { get; set; }
        public float Velocity { get; private set; }
        public VillageBehavior Village { get; set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();

            Body = new KinematicBodyComponent(new Sphere(5f, 1f));
            Parent.AddComponent(Body);
        }

        public override void Update(GameTime gameTime)
        {
            var left = InputManager.GetInput("left", PlayerIndex);
            var right = InputManager.GetInput("right", PlayerIndex);
            var up = InputManager.GetInput("up", PlayerIndex);
            var down = InputManager.GetInput("down", PlayerIndex);
            var sprint = InputManager.GetInput("sprint", PlayerIndex);

            var quit = InputManager.GetInput("quit", PlayerIndex);

            var direction = Vector3.Zero;
            if (quit.State == InputState.Down)
            {
                Game.Exit();
            }
            if (left.State == InputState.Down)
            {
                direction += Camera.Left;
            }
            if (right.State == InputState.Down)
            {
                direction += Camera.Right;
            }
            if (up.State == InputState.Down)
            {
                var forward = Camera.Forward;
                forward = new Vector3(forward.X, 0f, forward.Z);
                direction += forward;
            }
            if (down.State == InputState.Down)
            {
                var backward = Camera.Backward;
                backward = new Vector3(backward.X, 0f, backward.Z);
                direction += backward;
            }
            if (sprint.State == InputState.Down)
            {
                direction *= SprintFactor;
            }

            IsColliding = false;
            foreach (var pair in Body.Collisions)
            {
                var collision = pair.Value;
                if (collision.IsColliding)
                {
                    Parent.Position += (collision.Correction / Body.Collisions.Count);
                    IsColliding = true;
                }
            }
            Body.Move(direction * Velocity);
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}