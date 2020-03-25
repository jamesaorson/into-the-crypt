using Komodo.Core.ECS.Components;
using Komodo.Lib.Math;
using System;
using System.Diagnostics;

using GameTime = Microsoft.Xna.Framework.GameTime;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace Common.Behaviors
{
    public class EnemyBehavior : BehaviorComponent
    {

        #region Members

        #region Public Members
        public Vector3 Facing { get; set; }
        public PlayerBehavior Player { get; set; }
        public float SpriteAngleStep { get; set; }
        public int SpriteAngleFrame { get; set; }
        #endregion Public Members

        #endregion Members

        #region Member Methods

        #region Public Member Methods
        public override void Initialize()
        {
            base.Initialize();

            Parent.AddComponent(
                new SpriteComponent("player/idle/player_idle_01", Game?.DefaultSpriteShader)
                {
                    IsBillboard = true,
                }
            );
            Parent.Position = Player.WorldPosition;
            Facing = Vector3.Forward;
            SpriteAngleStep = 30f;
            SpriteAngleFrame = 0;
        }

        public override void Update(GameTime gameTime)
        {
            var target = (Player.WorldPosition - WorldPosition).MonoGameVector;
            target.Normalize();
            float dotProduct = Microsoft.Xna.Framework.Vector3.Dot(target, Facing.MonoGameVector);
            float direction = MathF.Round(Microsoft.Xna.Framework.Vector3.Cross(Facing.MonoGameVector, target).Y);
            float num = MathF.Acos(dotProduct);
            float angle = 180f - MathHelper.ToDegrees(num);
            if (float.IsNaN(angle))
            {
                SpriteAngleFrame = 0;
            }
            else
            {
                SpriteAngleFrame = (int)(direction * (angle / SpriteAngleStep));
            }
            /*Debug.WriteLine($"Direction: {direction}");
            Debug.WriteLine($"Angle: {angle}");
            Debug.WriteLine($"Sprite Angle Frame: {SpriteAngleFrame}\n");*/
        }
        #endregion Public Member Methods

        #endregion Member Methods
    }
}