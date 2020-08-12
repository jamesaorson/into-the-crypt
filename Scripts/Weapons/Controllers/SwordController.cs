using IntoTheCrypt.Models;

namespace IntoTheCrypt.Weapons.Controllers
{
	public class SwordController : WeaponController
	{
		#region Public

		#region Members
		public override WeaponStats Stats { get; protected set; } = new WeaponStats(
			baseDamage: 1,
			attackSpeed: 1f,
			quality: Quality.E,
			weaponClass: DamageClass.Sharp,
			toxicity: 10
		);
		#endregion

		#endregion
	}
}
