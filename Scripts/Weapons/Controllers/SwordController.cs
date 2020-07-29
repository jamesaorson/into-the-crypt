namespace IntoTheCrypt.Weapons.Controllers
{
	public class SwordController : WeaponController
	{
		#region Public

		#region Members
		public override WeaponStats Stats { get; protected set; } = new WeaponStats(
			baseDamage: 5,
			attackSpeed: 1f,
			bluntness: 0,
			sharpness: 10,
			toxicity: 10
		);
		#endregion

		#endregion
	}
}
