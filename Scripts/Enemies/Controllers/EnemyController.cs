using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using Godot;
using IntoTheCrypt.Player.Controllers;

namespace IntoTheCrypt.Enemies.Controllers
{
	public abstract class EnemyController : KinematicBody
	{
		#region Public

		#region Members
		public bool ShowDebugUI;
		public Label HealthText { get; protected set; }
		public Label ArmorText { get; protected set; }
		public Label BleedText { get; protected set; }
		public Label ToxicText { get; protected set; }
		public CSGMesh DebugInfo { get; protected set; }
		public MessageBus MessageBus { get; private set; }
		public Stats Stats { get; protected set; }
		public uint Sharpness { get; protected set; }
		public uint Toxicity { get; protected set; }
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
				var distance =  GlobalTransform.origin.DistanceTo(_player.GlobalTransform.origin);
				return distance <= AttackRange;
			}
		}
		public bool IsInTrackingRangeOfPlayer
		{
			get
			{
				var distance = GlobalTransform.origin.DistanceTo(_player.GlobalTransform.origin);
				return distance <= TrackingRange;
			}
		}

		public Vector3 TowardsPlayer2D
		{
			get
			{
				var direction = TowardsPlayer3D;
				direction.y = 0f;
				return direction.Normalized();
			}
		}

		public Vector3 TowardsPlayer3D
		{
			get
			{
				if (_player == null)
				{
					return Vector3.Zero;
				}
				var direction = _player.GlobalTransform.origin - GlobalTransform.origin;
				return direction.Normalized();
			}
		}
		#endregion

		#region Member Methods
		public void Die()
		{
			EmitSignal(nameof(MessageBus.EnemyDeath), GetInstanceId());
			QueueFree();
		}

		public void HandleDamage(DamageEnemyMessage damage)
		{
			GD.Print("Damaged...");
			DamageHelper.HandleDamage(Stats, damage);
		}

		public void Move(Vector3 normalizedDirection, float delta)
		{
			var translation = normalizedDirection * Speed;
			MoveAndSlide(translation);
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
		protected PlayerMoveController _player;
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
			PerformAttack();
		}

		protected void PerformAttack()
		{
			if (!IsInAttackRangeOfPlayer)
			{
				return;
			}
			MessageBus.EmitSignal(
				nameof(MessageBus.EnemyAttack),
				new DamagePlayerMessage(Stats, Quality.E, DamageClass.Blunt, 0)
			);
		}

		public override void _Ready()
		{
			DebugInfo = GetNode<CSGMesh>("DebugInfo");
			DebugInfo.Visible = false;
			MessageBus = GetNode<MessageBus>("/root/MessageBus");
			HealthText = DebugInfo.GetNode<Label>("DebugViewport/GridContainer/HealthLabel");
			ArmorText = DebugInfo.GetNode<Label>("DebugViewport/GridContainer/ArmorLabel");
			BleedText = DebugInfo.GetNode<Label>("DebugViewport/GridContainer/BleedLabel");
			ToxicText = DebugInfo.GetNode<Label>("DebugViewport/GridContainer/ToxicLabel");
			Stats.ArmorRating = Stats.MaxArmorRating;
			Stats.HP = Stats.MaxHP;
			var players = GetTree().GetNodesInGroup("player");
			if (players.Count == 0)
			{
				GD.PrintErr("Failed to find any players");
				GetTree().Quit();
			}
			_player = players[0] as PlayerMoveController;
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
			if (Input.IsActionJustPressed("toggle_debug"))
			{
				DebugInfo.Visible = !DebugInfo.Visible;
			}

			UpdateAttack(delta);
			UpdateBleed(delta);
			UpdateToxic(delta);
			TryDie();

			AiUpdate(delta);
			UpdateDebugText();
		}

		public override void _PhysicsProcess(float delta)
		{
			AiPhysicsUpdate(delta);
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
			HealthText.Text = $"{Stats.HP}/{Stats.MaxHP}";
			ArmorText.Text = $"{Stats.ArmorRating}/{Stats.MaxArmorRating}";
			BleedText.Text = $"{Stats.Bleed}";
			ToxicText.Text = $"{Stats.Toxic}";
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
		protected abstract void AiUpdate(float delta);
		protected abstract void AiPhysicsUpdate(float delta);
		#endregion

		#endregion
	}
}
