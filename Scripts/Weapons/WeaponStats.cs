using IntoTheCrypt.Models;

namespace IntoTheCrypt.Weapons
{
    public class WeaponStats
    {
        #region Public

        #region Constructors
        public WeaponStats(uint baseDamage, float attackSpeed, Quality quality, DamageClass weaponClass, uint toxicity)
            : this(
                new MajorWeaponStats(baseDamage: baseDamage, attackSpeed: attackSpeed),
                new MinorWeaponStats(quality: quality, weaponClass: weaponClass, toxicity: toxicity)
            )
        {
        }

        public WeaponStats(MajorWeaponStats majorStats, MinorWeaponStats minorStats)
        {
            _majorStats = majorStats;
            _minorStats = minorStats;
        }
        #endregion

        #region Members
        public Quality Quality
        {
            get => _minorStats.Quality;
            set
            {
                _minorStats.Quality = value;
            }
        }
        public DamageClass WeaponClass
        {
            get => _minorStats.WeaponClass;
            set
            {
                _minorStats.WeaponClass = value;
            }
        }
        public uint BaseDamage
        {
            get => _majorStats.BaseDamage;
            set
            {
                _majorStats.BaseDamage = value;
            }
        }
        public uint Toxicity
        {
            get => _minorStats.Toxicity;
            set
            {
                _minorStats.Toxicity = value;
            }
        }
        #endregion

        #endregion

        #region Private

        #region Members
        private MajorWeaponStats _majorStats;
        private MinorWeaponStats _minorStats;
        #endregion

        #endregion
    }
}