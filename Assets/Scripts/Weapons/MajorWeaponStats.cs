namespace IntoTheCrypt.Weapons
{
    public struct MajorWeaponStats
    {
        #region Public

        #region Constructors
        public MajorWeaponStats(uint baseDamage, float attackSpeed)
        {
            BaseDamage = baseDamage;
            if (attackSpeed <= 0f)
            {
                attackSpeed = 0f;
            }
            _attackSpeed = attackSpeed;
        }
        #endregion

        #region Members
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                if (value > 0f)
                {
                    _attackSpeed = value;
                }
                else
                {
                    _attackSpeed = 0f;
                }
            }
        }
        public uint BaseDamage { get; set; }
        #endregion

        #endregion

        #region Private

        #region Members
        private float _attackSpeed;
        #endregion

        #endregion
    }
}