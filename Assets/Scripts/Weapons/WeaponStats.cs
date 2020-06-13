namespace IntoTheCrypt.Weapons
{
    public class WeaponStats
    {
        #region Public

        #region Constructors
        public WeaponStats(uint baseDamage, float attackSpeed, uint bluntness, uint sharpness, uint toxicity)
            : this(
                new MajorWeaponStats(baseDamage: baseDamage, attackSpeed: attackSpeed),
                new MinorWeaponStats(bluntness: bluntness, sharpness: sharpness, toxicity: toxicity)
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
        public uint BaseDamage
        {
            get => _majorStats.BaseDamage;
            set
            {
                _majorStats.BaseDamage = value;
            }
        }
        public uint Bluntness
        {
            get => _minorStats.Bluntness;
            set
            {
                _minorStats.Bluntness = value;
            }
        }
        public uint Sharpness
        {
            get => _minorStats.Sharpness;
            set
            {
                _minorStats.Sharpness = value;
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