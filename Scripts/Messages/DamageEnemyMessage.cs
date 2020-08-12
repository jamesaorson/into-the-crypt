using IntoTheCrypt.Models;
using IntoTheCrypt.Weapons;

namespace IntoTheCrypt.Messages
{
    public class DamageEnemyMessage : IDamageMessage
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

        public uint Damage => Weapon.BaseDamage;
        public Quality Quality => Weapon.Quality;
        public uint Toxicity => Weapon.Toxicity;
        public DamageClass DamageClass => Weapon.WeaponClass;
        #endregion

        #endregion
    }
}