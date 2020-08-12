using IntoTheCrypt.Models;

namespace IntoTheCrypt.Weapons
{
    public struct MinorWeaponStats
    {
        #region Public

        #region Constructors
        public MinorWeaponStats(Quality quality, DamageClass weaponClass, uint toxicity)
        {
            WeaponClass = weaponClass;
            Quality = quality;
            Toxicity = toxicity;
        }
        #endregion

        #region Members
        public Quality Quality { get; set; }
        public DamageClass WeaponClass { get; set; }
        public uint Toxicity { get; set; }
        #endregion

        #endregion
    }
}