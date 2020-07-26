using Godot;
using IntoTheCrypt.Player.Controllers;

namespace IntoTheCrypt.Weapons.Sword.Controllers
{
    public class SwordDamageController : Node
    {
        #region Public

        #region Members
        public PlayerController Player { get; protected set; }
        public WeaponStats Stats = new WeaponStats(
            baseDamage: 10,
            attackSpeed: 1f,
            bluntness: 0,
            sharpness: 10,
            toxicity: 10
        );
        #endregion

        #region Member Methods
        public void SetPlayer(PlayerController player)
        {
            Player = player;
        }
        #endregion

        #endregion

        #region Private

        #region Member Methods
        /*private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ENEMY_TAG))
            {
                other.SendMessage("HandleDamage", new DamageEnemyMessage(Player.Stats, Stats));
            }
        }*/
        #endregion

        #endregion
    }
}