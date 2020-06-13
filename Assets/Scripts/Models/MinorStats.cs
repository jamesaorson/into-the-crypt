namespace IntoTheCrypt.Models
{
    [System.Serializable]
    public struct MinorStats
    {
        #region Public

        #region Constructors
        public MinorStats(uint bleedResistance, uint toxicResistance)
        {
            BleedResistance = bleedResistance;
            ToxicResistance = toxicResistance;
        }
        #endregion

        #region Members
        public uint BleedResistance;
        public uint ToxicResistance;
        #endregion

        #endregion
    }
}