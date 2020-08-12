using IntoTheCrypt.Models;

namespace IntoTheCrypt.Messages
{
    public interface IDamageMessage
    {
        #region Public

        #region Members
        uint Damage { get; }
        Quality Quality { get; }
        DamageClass DamageClass { get; }
        uint Toxicity { get; }
        #endregion

        #endregion
    }
}