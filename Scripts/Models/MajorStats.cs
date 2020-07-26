using Godot;

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
        [Export]
        public uint Dexterity;
        [Export]
        public uint Strength;
        #endregion

        #endregion
    }
}