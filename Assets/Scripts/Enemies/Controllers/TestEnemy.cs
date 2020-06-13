using IntoTheCrypt.Models;

namespace IntoTheCrypt.Enemies.Controllers
{
    public class TestEnemy : EnemyController
    {
        #region Protected
        
        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused protected members", Justification = "<Pending>")]
        protected override void Start()
        {
            base.Start();

            Stats.MaxHP = 200;
            Stats.HP = Stats.MaxHP;
            Stats.MaxArmorRating = 20;
            Stats.ArmorRating = Stats.MaxArmorRating;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused protected members", Justification = "<Pending>")]
        protected override void Update()
        {
            base.Update();
        }
        #endregion

        #endregion
    }
}