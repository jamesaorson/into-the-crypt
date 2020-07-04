namespace IntoTheCrypt.Enemies.Controllers
{
    public class Squog : EnemyController
    {
        #region Protected

        #region Member Methods
        protected override void AIUpdate()
        {
            if (IsInAttackRangeOfPlayer)
            {
                Attack();
            }
            if (IsInTrackingRangeOfPlayer)
            {
                Move(TowardsPlayer2D);
            }
        }
        #endregion

        #endregion
    }
}