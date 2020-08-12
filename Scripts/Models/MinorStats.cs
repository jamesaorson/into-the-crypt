using Godot;

namespace IntoTheCrypt.Models
{
    [System.Serializable]
    public struct MinorStats
    {
        #region Public

        #region Constructors
        public MinorStats(uint coagulation, uint toxicResistance)
        {
            Coagulation = coagulation;
            ToxicResistance = toxicResistance;
        }
        #endregion

        #region Members
        [Export]
        public uint Coagulation;
        [Export]
        public uint ToxicResistance;
        #endregion

        #endregion
    }
}