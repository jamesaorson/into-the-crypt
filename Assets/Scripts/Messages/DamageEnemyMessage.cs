using IntoTheCrypt.Models;
using IntoTheCrypt.Weapons;

namespace IntoTheCrypt.Messages
{
    public class DamageEnemyMessage : DamageMessage
    {
        #region Public

        #region Constructors
        public DamageEnemyMessage(Stats player, WeaponStats weapon)
        {
            Player = player;
            Weapon = weapon;
        }
        #endregion

        #region Members
        public WeaponStats Weapon { get; private set; }
        public Stats Player { get; private set; }

        public override uint Bluntness => Weapon.Bluntness;
        public override uint Damage => Weapon.BaseDamage;
        public override uint Sharpness => Weapon.Sharpness;
        public override uint Toxicity => Weapon.Toxicity;
        #endregion

        #endregion
    }
}