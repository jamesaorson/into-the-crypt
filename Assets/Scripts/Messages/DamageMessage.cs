namespace IntoTheCrypt.Messages
{
    public abstract class DamageMessage
    {
        #region Public

        #region Members
        public abstract uint Bluntness { get; }
        public abstract uint Damage { get; }
        public abstract uint Sharpness { get; }
        public abstract uint Toxicity { get; }
        #endregion

        #endregion
    }
}