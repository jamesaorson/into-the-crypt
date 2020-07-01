using UnityEngine;

namespace IntoTheCrypt.Enemies.Controllers
{
    public class Squog : EnemyController
    {
        #region Public
        
        #region Members
        public GameObject Player;
        #endregion
        
        #endregion
        
        #region Protected
        
        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused protected members", Justification = "<Pending>")]
        protected override void Start()
        {
            base.Start();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused protected members", Justification = "<Pending>")]
        protected override void Update()
        {
            base.Update();
            
            Move(TowardsPlayer2D);
        }
        #endregion

        #endregion
    }
}