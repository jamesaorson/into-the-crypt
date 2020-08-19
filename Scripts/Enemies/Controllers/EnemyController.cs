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
		public AnimatedSprite3D Sprite { get; protected set; }
		public CSGMesh DebugInfo { get; protected set; }
		public MessageBus MessageBus { get; private set; }
		[Export]
		public Stats Stats { get; protected set; }
		public uint Sharpness { get; protected set; }
		public uint Toxicity { get; protected set; }
		[Export]
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
		[Export]
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
		public bool StartedAttacking { get; set; }
		public bool PerformedAttack { get; set; }
		public bool SentDeathSignal { get; set; }
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
		public override void _PhysicsProcess(float delta)
		{
			AiPhysicsUpdate(delta);
		}

		public override void _Process(float delta)
		{
			if (Input.IsActionJustPressed("toggle_debug"))
			{
				DebugInfo.Visible = !DebugInfo.Visible;
			}

			UpdateBleed(delta);
			UpdateToxic(delta);
			TryDie();

			AiUpdate(delta);
			UpdateDebugText();
		}

		public override void _Ready()
		{
			DebugInfo = GetNode<CSGMesh>("DebugInfo");
			Sprite = GetNode<AnimatedSprite3D>("Sprite");
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

			ConnectSignals();
		}

		public void Die()
		{
			if (SentDeathSignal)
			{
				return;
			}
		 	MessageBus.EmitSignal(nameof(MessageBus.EnemyDeath), GetInstanceId());
			SentDeathSignal = true;
			QueueFree();
		}

		public void HandleDamage(DamageEnemyMessage damage)
		{
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
		protected float _attackRange = 0f;
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
			StartedAttacking = true;
			PerformedAttack = false;
			Sprite.Animation = "attack";
		}

		protected abstract void ConnectSignals();

		protected abstract void HandleFrameChanged();

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
			PerformedAttack = true;
		}

		protected void TryDie()
		{
			if (Stats.HP <= 0f)
			{
				Die();
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
