using IntoTheCrypt.Models;
using UnityEngine;

namespace IntoTheCrypt.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region Public

        #region Members
        public Stats Stats;
        #endregion

        #endregion
        
        #region Private

        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {
            Stats.ArmorRating = Stats.MaxArmorRating;
            Stats.HP = Stats.MaxHP;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {
        }
        #endregion

        #endregion
    }
}