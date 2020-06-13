namespace IntoTheCrypt.Models
{
    public struct Health
    {
        #region Public

        #region Constructors
        public Health(uint maxHp)
        {
            _hp = maxHp;
            _maxHp = maxHp;
        }
        #endregion

        #region Members
        public float HP
        {
            get => _hp;
            set
            {
                if (value <= _maxHp)
                {
                    _hp = value;
                }
                else
                {
                    _hp = _maxHp;
                }
            }
        }

        public float MaxHP
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                if (value < _hp)
                {
                    HP = MaxHP;
                }
            }
        }
        #endregion

        #endregion

        #region Private

        #region Members
        private float _hp;
        private float _maxHp;
        #endregion

        #endregion
    }
}