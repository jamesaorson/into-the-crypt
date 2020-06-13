using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using UnityEngine;

namespace IntoTheCrypt.Enemies.Controllers
{
    public abstract class EnemyController : MonoBehaviour
    {
        #region Public

        #region Members
        [Tooltip("Text for health debugging")]
        public TextMesh HealthText;
        [Tooltip("Text for bleed debugging")]
        public TextMesh BleedText;
        [Tooltip("Text for toxic debugging")]
        public TextMesh ToxicText;
        public bool IsBleeding => Stats == null ? false : Stats.IsBleeding;
        public Stats Stats;

        #endregion

        #region Member Methods
        public void Die()
        {
            Destroy(gameObject);
        }

        public void HandleDamage(DamageEnemyMessage damage)
        {
            DamageHelper.HandleDamage(Stats, damage);
        }
        #endregion

        #endregion

        #region Protected

        #region Members
        protected float _bleedTime = 0f;
        protected float _toxicTime = 0f;
        #endregion

        #region Member Methods
        protected virtual void Start()
        {
        }

        protected void TryDie()
        {
            if (Stats.HP <= 0f)
            {
                Die();
            }
        }

        protected virtual void Update()
        {
            UpdateBleed();
            UpdateToxic();
            TryDie();

#if UNITY_EDITOR
            UpdateDebugText();
#endif
        }

        protected void UpdateBleed()
        {
            if (Stats.Bleed == 0f)
            {
                _bleedTime = 0f;
                return;
            }
            _bleedTime += Time.deltaTime;
            // Accumulate bleed damage
            uint accumulatedDamage = 0;
            for (int i = 1; i <= _bleedTime; ++i)
            {
                accumulatedDamage += DamageHelper.DamageFromBleed(Stats);
                Stats.Bleed *= Stats.BleedReductionRatio;
            }
            if (Stats.Bleed <= 1f)
            {
                Stats.Bleed = 0f;
            }
            // Remove excess seconds that have passed since last update
            _bleedTime %= 1f;
            DamageHelper.Damage(Stats, accumulatedDamage);
        }

        protected void UpdateDebugText()
        {
            HealthText.text = $"HP: {Stats.HP}/{Stats.MaxHP}";
            BleedText.text = $"Bleed: {Stats.Bleed}";
            ToxicText.text = $"Toxic: {Stats.Toxic}";
        }

        protected void UpdateToxic()
        {
            var transientToxic = Stats.TransientToxic;
            if (transientToxic == 0f)
            {
                _toxicTime = 0f;
                return;
            }
            _toxicTime += Time.deltaTime;
            // Reduce toxic
            uint toxicToRemove = 0;
            for (int i = 1; i <= _toxicTime; ++i)
            {
                toxicToRemove += Stats.TOXIC_DROP_RATE;
            }
            if (toxicToRemove > transientToxic)
            {
                toxicToRemove = transientToxic;
            }
            // Remove the transient toxicity
            Stats.Toxic -= toxicToRemove;
            // Remove excess seconds that have passed since last update
            _toxicTime %= 1f;
        }
        #endregion

        #endregion
    }
}
