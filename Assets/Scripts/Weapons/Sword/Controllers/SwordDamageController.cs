using IntoTheCrypt.Messages;
using IntoTheCrypt.Player.Controllers;
using UnityEngine;

namespace IntoTheCrypt.Weapons.Sword.Controllers
{
    public class SwordDamageController : MonoBehaviour
    {
        #region Public

        #region Members
        [Tooltip("PlayerController of the Player who has equipped the weapon")]
        public PlayerController Player;
        public WeaponStats Stats = new WeaponStats(
            baseDamage: 10,
            attackSpeed: 1f,
            bluntness: 0,
            sharpness: 10,
            toxicity: 10
        );
        #endregion

        #endregion

        #region Private

        #region Member Methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ENEMY_TAG))
            {
                other.SendMessage("HandleDamage", new DamageEnemyMessage(Player.Stats, Stats));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {

        }
        #endregion

        #endregion
    }
}