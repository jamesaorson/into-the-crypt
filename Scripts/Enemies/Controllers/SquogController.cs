namespace IntoTheCrypt.Enemies.Controllers
{
    public class SquogController : EnemyController
    {
        #region Protected

        #region Member Methods
        protected override void AIUpdate(float delta)
        {
            if (IsInAttackRangeOfPlayer)
            {
                Attack();
            }
            if (IsInTrackingRangeOfPlayer)
            {
                Move(TowardsPlayer2D, delta);
            }
        }
        #endregion

        #endregion
    }
}