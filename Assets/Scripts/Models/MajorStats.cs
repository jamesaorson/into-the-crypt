namespace IntoTheCrypt.Models
{
    [System.Serializable]
    public struct MajorStats
    {
        #region Public

        #region Constructors
        public MajorStats(uint dexterity, uint strength)
        {
            Dexterity = dexterity;
            Strength = strength;
        }
        #endregion

        #region Members
        public uint Dexterity;
        public uint Strength;
        #endregion

        #endregion
    }
}