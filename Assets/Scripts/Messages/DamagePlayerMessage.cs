using IntoTheCrypt.Models;

namespace IntoTheCrypt.Messages
{
    public class DamagePlayerMessage : DamageMessage
    {
        #region Public

        #region Constructors
        public DamagePlayerMessage(Stats enemy)
        {
            Enemy = enemy;
        }
        #endregion

        #region Members
        public Stats Enemy { get; private set; }
        public override uint Bluntness => 0;
        public override uint Damage => Enemy.Strength;
        public override uint Sharpness => 0;
        public override uint Toxicity => 0;
        #endregion

        #endregion
    }
}