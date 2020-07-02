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
        protected override void AIUpdate()
        {
            if (IsInAttackRangeOfPlayer)
            {
                Attack();
            }
            Move(TowardsPlayer2D);
        }
        #endregion

        #endregion
    }
}