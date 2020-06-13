namespace IntoTheCrypt.Weapons
{
    public struct MinorWeaponStats
    {
        #region Public

        #region Constructors
        public MinorWeaponStats(uint bluntness, uint sharpness, uint toxicity)
        {
            Bluntness = bluntness;
            Sharpness = sharpness;
            Toxicity = toxicity;
        }
        #endregion

        #region Members
        public uint Bluntness { get; set; }
        public uint Sharpness { get; set; }
        public uint Toxicity { get; set; }
        #endregion

        #endregion
    }
}