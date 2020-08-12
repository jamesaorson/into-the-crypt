using IntoTheCrypt.Models;

namespace IntoTheCrypt.Messages
{
    public class DamagePlayerMessage : IDamageMessage
    {
        #region Public

        #region Constructors
        public DamagePlayerMessage(Stats enemy, Quality quality, DamageClass damageClass, uint toxicity)
        {
            Enemy = enemy;
            _damageClass = damageClass;
            _quality = quality;
            _toxicity = toxicity;
        }
        #endregion

        #region Members
        public Stats Enemy { get; private set; }
        public Quality Quality => _quality;
        public uint Damage => Enemy.Strength;
        public DamageClass DamageClass => _damageClass;
        public uint Toxicity => _toxicity;
        #endregion

        #endregion

        #region Private

        #region Members
        private DamageClass _damageClass { get; set; }
        private Quality _quality { get; set; }
        private uint _toxicity { get; set; }
        #endregion

        #endregion
    }
}