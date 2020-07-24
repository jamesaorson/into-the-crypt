using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntoTheCrypt.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        #region Public

        #region Members
        [Tooltip("Stat menu")]
        public StatMenuController StatMenu;
        public Stats Stats;
        #endregion

        #region Member Methods
        public void HandleDamage(DamagePlayerMessage damage)
        {
            DamageHelper.HandleDamage(Stats, damage);
        }
        #endregion

        #endregion
        
        #region Private

        #region Members
        protected float _bleedElapsedTime = 0f;
        protected float _toxicElapsedTime = 0f;
        #endregion

        #region Member Methods
        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
                return;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                StatMenu.ToggleActive();
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Start()
        {
            StatMenu.SetActive(false);
            Stats.ArmorRating = Stats.MaxArmorRating;
            Stats.HP = Stats.MaxHP;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {
            UpdateBleed();
            UpdateToxic();
            UpdateMenus();

            HandleInput();
        }

        private void UpdateBleed()
        {
            if (Stats.Bleed == 0f)
            {
                _bleedElapsedTime = 0f;
                return;
            }
            _bleedElapsedTime += Time.deltaTime;
            // Accumulate bleed damage
            uint accumulatedDamage = 0;
            for (int i = 1; i <= _bleedElapsedTime; ++i)
            {
                accumulatedDamage += DamageHelper.DamageFromBleed(Stats);
                Stats.Bleed *= Stats.BleedReductionRatio;
            }
            if (Stats.Bleed <= 1f)
            {
                Stats.Bleed = 0f;
            }
            // Remove excess seconds that have passed since last update
            _bleedElapsedTime %= 1f;
            DamageHelper.Damage(Stats, accumulatedDamage);
        }

        private void UpdateMenus()
        {
            UpdateStatMenu();
        }

        private void UpdateStatMenu()
        {
            StatMenu.UpdateStats(Stats);
        }

        private void UpdateToxic()
        {
            var transientToxic = Stats.TransientToxic;
            if (transientToxic == 0f)
            {
                _toxicElapsedTime = 0f;
                return;
            }
            _toxicElapsedTime += Time.deltaTime;
            // Reduce toxic
            uint toxicToRemove = 0;
            for (int i = 1; i <= _toxicElapsedTime; ++i)
            {
                toxicToRemove += Constants.TOXIC_DROP_RATE;
            }
            if (toxicToRemove > transientToxic)
            {
                toxicToRemove = transientToxic;
            }
            // Remove the transient toxicity
            Stats.Toxic -= toxicToRemove;
            // Remove excess seconds that have passed since last update
            _toxicElapsedTime %= 1f;
        }
        #endregion

        #endregion
    }
}