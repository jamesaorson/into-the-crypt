using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using TMPro;
using UnityEngine;

namespace IntoTheCrypt.Enemies.Controllers
{
    public abstract class EnemyController : MonoBehaviour
    {
        #region Public

        #region Members
        public bool ShowDebugUI;
        [Tooltip("Character controller used for movement.")]
        public CharacterController Character;
        [Tooltip("Text for health debugging")]
        public TextMeshPro HealthText;
        [Tooltip("Text for bleed debugging")]
        public TextMeshPro BleedText;
        [Tooltip("Text for toxic debugging")]
        public TextMeshPro ToxicText;
        public Stats Stats;
        public uint Sharpness = 0;
        public uint Toxicity = 0;
        public float AttackDelay
        {
            get => _attackDelay;
            set
            {
                if (value < 0f)
                {
                    value = 0f;
                }
                _attackDelay = value;
            }
        }
        public float AttackRange
        {
            get => _attackRange;
            set
            {
                if (value < 0f)
                {
                    value = 0f;
                }
                _attackRange = value;
            }
        }
        public float TrackingRange
        {
            get => _trackingRange;
            set
            {
                if (value < 0f)
                {
                    value = 0f;
                }
                _trackingRange = value;
            }
        }
        public bool IsAttacking { get; set; }
        public bool IsBleeding => Stats == null ? false : Stats.IsBleeding;
        public float Speed => Stats == null ? 0f : Stats.Dexterity * Constants.DEXTERITY_TO_SPEED_FACTOR;
        public bool IsInAttackRangeOfPlayer
        {
            get
            {
                var distance = Vector3.Distance(transform.position, _player.transform.position);
                return distance <= AttackRange;
            }
        }
        public bool IsInTrackingRangeOfPlayer
        {
            get
            {
                var distance = Vector3.Distance(transform.position, _player.transform.position);
                return distance <= TrackingRange;
            }
        }
        // Does not include the Y component of the direction to the player.
        public Vector3 TowardsPlayer2D
        {
            get
            {
                var direction = TowardsPlayer3D;
                direction.y = 0f;
                return direction;
            }
        }
        // Includes the Y component of the direction to the player.
        public Vector3 TowardsPlayer3D
        {
            get
            {
                if (_player == null)
                {
                    return Vector3.zero;
                }
                var direction = _player.transform.position - transform.position;
                return direction.normalized;
            }
        }
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

        public void Move(Vector3 normalizedDirection)
        {
            var translation = normalizedDirection * Speed * Time.deltaTime;
            Character.Move(translation);
        }
        #endregion

        #endregion

        #region Protected

        #region Members
        [SerializeField]
        [Min(0f)]
        [Tooltip("Delay in seconds between starting an attack and the actual attack checking for a hit")]
        protected float _attackDelay = 0f;
        protected float _attackElapsedTime = 0f;
        [SerializeField]
        [Min(0f)]
        [Tooltip("Range of enemy attacks")]
        protected float _attackRange = 0f;
        [SerializeField]
        [Min(0f)]
        [Tooltip("Range of tracking")]
        protected float _trackingRange = 0f;
        protected float _bleedElapsedTime = 0f;
        protected GameObject _player;
        protected float _toxicElapsedTime = 0f;
        #endregion

        #region Member Methods
        protected void Attack()
        {
            if (IsAttacking)
            {
                return;
            }
            IsAttacking = true;
        }

        protected void PerformAttack()
        {
            if (!IsInAttackRangeOfPlayer)
            {
                return;
            }
            _player.SendMessage("HandleDamage", new DamagePlayerMessage(Stats, Sharpness, Toxicity));
        }

        protected void Start()
        {
            Stats.ArmorRating = Stats.MaxArmorRating;
            Stats.HP = Stats.MaxHP;
            _player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        }

        protected void TryDie()
        {
            if (Stats.HP <= 0f)
            {
                Die();
            }
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowDebugUI = !ShowDebugUI;
            }

            UpdateAttack();
            UpdateBleed();
            UpdateToxic();
            TryDie();

            AIUpdate();

#if UNITY_EDITOR
            UpdateDebugText();
#endif
        }

        protected void UpdateAttack()
        {
            if (!IsAttacking)
            {
                _attackElapsedTime = 0f;
                return;
            }
            _attackElapsedTime += Time.deltaTime;

            if (_attackElapsedTime >= AttackDelay)
            {
                _attackElapsedTime = 0f;
                IsAttacking = false;

                PerformAttack();
            }
        }

        protected void UpdateBleed()
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

        protected void UpdateDebugText()
        {
            if (!ShowDebugUI)
            {
                HealthText.text = "";
                BleedText.text = "";
                ToxicText.text = "";
                return;
            }
            HealthText.text = $"HP: {Stats.HP}/{Stats.MaxHP}";
            BleedText.text = $"Bleed: {Stats.Bleed}";
            ToxicText.text = $"Toxic: {Stats.Toxic}";
        }

        protected void UpdateToxic()
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

        #region Abstract Member Methods
        protected abstract void AIUpdate();
        #endregion

        #endregion
    }
}
