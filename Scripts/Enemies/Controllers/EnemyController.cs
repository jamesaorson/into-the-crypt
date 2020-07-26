using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using Godot;

namespace IntoTheCrypt.Enemies.Controllers
{
    public abstract class EnemyController : Spatial
    {
        #region Public

        #region Members
        public bool ShowDebugUI;
        //public CharacterController Character;
        //public Animator Animator;
        public Label HealthText;
        public Label BleedText;
        public Label ToxicText;
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
                var distance =  Translation.DistanceTo(_player.Translation);
                return distance <= AttackRange;
            }
        }
        public bool IsInTrackingRangeOfPlayer
        {
            get
            {
                var distance = Translation.DistanceTo(_player.Translation);
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
                    return Vector3.Zero;
                }
                var direction = _player.Translation - Translation;
                return direction.Normalized();
            }
        }
        #endregion

        #region Member Methods
        public void Die()
        {
            //Destroy(gameObject);
        }

        public void HandleDamage(DamageEnemyMessage damage)
        {
            DamageHelper.HandleDamage(Stats, damage);
        }

        public void Move(Vector3 normalizedDirection, float delta)
        {
            var translation = normalizedDirection * Speed * delta;
            //Character.Move(translation);
        }
        #endregion

        #endregion

        #region Protected

        #region Members
        // [SerializeField]
        // [Min(0f)]
        // [Tooltip("Delay in seconds between starting an attack and the actual attack checking for a hit")]
        protected float _attackDelay = 0f;
        protected float _attackElapsedTime = 0f;
        // [SerializeField]
        // [Min(0f)]
        // [Tooltip("Range of enemy attacks")]
        protected float _attackRange = 0f;
        // [SerializeField]
        // [Min(0f)]
        // [Tooltip("Range of tracking")]
        protected float _trackingRange = 0f;
        protected float _bleedElapsedTime = 0f;
        protected Spatial _player;
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
            //Animator.SetTrigger("Attack");
        }

        protected void PerformAttack()
        {
            if (!IsInAttackRangeOfPlayer)
            {
                return;
            }
            //_player.SendMessage("HandleDamage", new DamagePlayerMessage(Stats, Sharpness, Toxicity));
        }

        public override void _Ready()
        {
            Stats.ArmorRating = Stats.MaxArmorRating;
            Stats.HP = Stats.MaxHP;
            //_player = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG);
        }

        protected void TryDie()
        {
            if (Stats.HP <= 0f)
            {
                Die();
            }
        }

        public override void _Process(float delta)
        {
            /*if (Input.GetKeyDown(KeyCode.F1))
            {
                ShowDebugUI = !ShowDebugUI;
            }*/

            UpdateAttack(delta);
            UpdateBleed(delta);
            UpdateToxic(delta);
            TryDie();

            AIUpdate(delta);

#if UNITY_EDITOR
            UpdateDebugText();
#endif
        }

        protected void UpdateAttack(float delta)
        {
            if (!IsAttacking)
            {
                _attackElapsedTime = 0f;
                return;
            }
            _attackElapsedTime += delta;

            if (_attackElapsedTime >= AttackDelay)
            {
                _attackElapsedTime = 0f;
                IsAttacking = false;

                PerformAttack();
            }
        }

        protected void UpdateBleed(float delta)
        {
            if (Stats.Bleed == 0f)
            {
                _bleedElapsedTime = 0f;
                return;
            }
            _bleedElapsedTime += delta;
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
                HealthText.Text = "";
                BleedText.Text = "";
                ToxicText.Text = "";
                return;
            }
            HealthText.Text = $"HP: {Stats.HP}/{Stats.MaxHP}";
            BleedText.Text = $"Bleed: {Stats.Bleed}";
            ToxicText.Text = $"Toxic: {Stats.Toxic}";
        }

        protected void UpdateToxic(float delta)
        {
            var transientToxic = Stats.TransientToxic;
            if (transientToxic == 0f)
            {
                _toxicElapsedTime = 0f;
                return;
            }
            _toxicElapsedTime += delta;
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
        protected abstract void AIUpdate(float delta);
        #endregion

        #endregion
    }
}
