using IntoTheCrypt.Models;

namespace IntoTheCrypt.Messages
{
    public class DamagePlayerMessage : DamageMessage
    {
        #region Public

        #region Constructors
        public DamagePlayerMessage(Stats enemy, uint sharpness, uint toxicity)
        {
            Enemy = enemy;
            _sharpness = sharpness;
            _toxicity = toxicity;
        }
        #endregion

        #region Members
        public Stats Enemy { get; private set; }
        public override uint Bluntness => 0;
        public override uint Damage => Enemy.Strength;
        public override uint Sharpness => _sharpness;
        public override uint Toxicity => _toxicity;
        #endregion

        #endregion

        #region Private

        #region Members
        private uint _sharpness { get; set; }
        private uint _toxicity { get; set; }
        #endregion

        #endregion
    }
}