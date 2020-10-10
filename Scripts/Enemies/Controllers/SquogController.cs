using System;

namespace IntoTheCrypt.Enemies.Controllers
{
	public class SquogController : EnemyController
	{
		#region Public

		#region Member Methods
		public override void _Ready()
		{
			Sharpness = 0;
			Toxicity = 0;

			base._Ready();
		}
		#endregion

		#endregion

		#region Protected

		#region Member Methods
		protected override void AiUpdate(float delta)
		{
			if (IsInAttackRangeOfPlayer)
			{
				Attack();
			}

			if (IsAttacking)
			{
				HandleFrameChanged();
			}
		}

		protected override void AiPhysicsUpdate(float delta)
		{
			if (IsInTrackingRangeOfPlayer)
			{
				Move(TowardsPlayer2D, delta);
			}
		}

		protected override void ConnectSignals()
		{
			Sprite.Connect("frame_changed", this, nameof(HandleFrameChanged));
		}

		protected override void HandleFrameChanged()
		{
			switch (Sprite.Animation)
			{
				case "attack":
					if (StartedAttacking && Sprite.Frame != 0)
					{
						StartedAttacking = false;
					}
					if (!PerformedAttack && Sprite.Frame == 1)
					{
						PerformAttack();
					}
					if (!StartedAttacking && Sprite.Frame == 0)
					{
						Sprite.Animation = "idle";
						IsAttacking = false;
					}
					break;
				case "idle":
					break;
				default:
					throw new Exception("Invalid animation");
			}
		}
		#endregion

		#endregion
	}
}
