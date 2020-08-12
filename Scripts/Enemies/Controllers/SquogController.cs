using IntoTheCrypt.Models;

namespace IntoTheCrypt.Enemies.Controllers
{
	public class SquogController : EnemyController
	{
		#region Public

		#region Member Methods
		public override void _Ready()
		{
			Stats = new Stats(
				maxHp: 100,
				maxArmorRating: 5,
				dexterity: 5,
				strength: 1,
				coagulation: 0,
				toxicResistance: 0
			);
			Sharpness = 0;
			Toxicity = 0;
			AttackRange = 1f;
			TrackingRange = 5f;

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
				//Attack();
			}
		}

		protected override void AiPhysicsUpdate(float delta)
		{
			if (IsInTrackingRangeOfPlayer)
			{
				Move(TowardsPlayer2D, delta);
			}
		}
		#endregion

		#endregion
	}
}
