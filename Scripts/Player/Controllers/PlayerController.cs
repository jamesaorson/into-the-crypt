using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using IntoTheCrypt.Enemies.Controllers;
using IntoTheCrypt.Helpers;
using IntoTheCrypt.Messages;
using IntoTheCrypt.Models;
using IntoTheCrypt.UI.StatMenu;
using IntoTheCrypt.Weapons.Controllers;

namespace IntoTheCrypt.Player.Controllers
{
	public class PlayerController : Spatial
	{
		#region Public

		#region Members
		[Export(PropertyHint.Range, "10,300")]
		public float LanternInterval = 10f;
		public CollisionShape HitBox { get; private set; }
		public Stopwatch Clock { get; private set; }
		public OmniLight Lantern { get; private set; }
		public MessageBus MessageBus { get; private set; }
		public StatMenuController StatMenu { get; private set; }
		public Stats Stats { get; private set; }
		public WeaponController Weapon { get; private set; }
		#endregion

		#region Member Methods
		public override void _Input(InputEvent @event)
		{
			switch (@event)
			{
				case InputEventKey key:
					switch (key.Scancode)
					{
						case (uint)KeyList.Escape:
							if (key.IsPressed())
							{
								GetTree().ChangeSceneTo(_mainMenuScene);
							}
							break;
						case (uint)KeyList.Tab:
							if (key.IsPressed())
							{
								StatMenu.ToggleActive();
							}
							break;
						default:
							break;
					}
					break;
				default:
					break;
			}
		}
		
		public override void _Process(float delta)
		{
			HandleInput(delta);
			UpdateBleed(delta);
			UpdateToxic(delta);
			UpdateMenus(delta);
			//UpdateLantern(delta);
		}
		
		public override void _Ready()
		{
			LoadScenes();
			_enemiesInRange = new Dictionary<ulong, EnemyController>();
			MessageBus = GetNode<MessageBus>("/root/MessageBus");
			StatMenu = GetNode<StatMenuController>("StatMenuControl");
			HitBox = GetNode<CollisionShape>("CameraContainer/Camera/Hand/Area/HitBox");
			Weapon = GetNode<Spatial>("CameraContainer/Camera/Hand").GetChildOrNull<WeaponController>(0);
			Lantern = GetNode<OmniLight>("Lantern");
			_startingLanternRange = Lantern.OmniRange;

			Stats = new Stats(
				maxArmorRating: 1,
				maxHp: 100
			);
			StatMenu.SetActive(false);
			Stats.ArmorRating = Stats.MaxArmorRating;
			Stats.HP = Stats.MaxHP;
			
			Clock = Stopwatch.StartNew();

			ConnectSignals();
		}
		
		public void HandleDamage(DamagePlayerMessage damage)
		{
			DamageHelper.HandleDamage(Stats, damage);
		}
		#endregion

		#endregion
		
		#region Private

		#region Members
		private float _bleedElapsedTime = 0f;
		private float MILLISECONDS_TO_SECOND = 1000f;
		private PackedScene _mainMenuScene;
		private float _startingLanternRange;
		private float _toxicElapsedTime = 0f;
		private IDictionary<ulong, EnemyController> _enemiesInRange;
		#endregion

		#region Member Methods
		private void HandleEnemyAttack(DamagePlayerMessage damage)
		{
			DamageHelper.HandleDamage(Stats, damage);
		}

		private void HandleEnemyDeath(ulong instanceId)
		{
			_enemiesInRange.Remove(instanceId);
		}

		private void HandleInput(float delta)
		{
			if (Input.IsActionJustPressed("attack"))
			{
				foreach (var item in _enemiesInRange)
				{
					var enemy = item.Value;
					enemy?.HandleDamage(new DamageEnemyMessage(Stats, Weapon.Stats));
				}
			}
		}

		private void ConnectSignals()
		{
			MessageBus.Connect(nameof(MessageBus.EnemyDeath), this, nameof(HandleEnemyDeath));
			MessageBus.Connect(nameof(MessageBus.EnemyAttack), this, nameof(HandleEnemyAttack));
		}

		private void LoadScenes()
		{
			_mainMenuScene = GD.Load<PackedScene>(Constants.ResourceMainMenuUI);
			if (_mainMenuScene == null)
			{
				throw new Exception("MainMenu scene did not load correctly");
			}
		}

		private void OnHitBoxEnter(EnemyController body)
		{
			_enemiesInRange[body.GetInstanceId()] = body;
		}

		private void OnHitBoxExit(EnemyController body)
		{
			_enemiesInRange.Remove(body.GetInstanceId());
		}

		private void UpdateBleed(float delta)
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
		
		private void UpdateLantern(float delta)
		{
			Lantern.OmniRange = Mathf.Clamp(
				_startingLanternRange - (Clock.ElapsedMilliseconds / (LanternInterval * MILLISECONDS_TO_SECOND)),
				0f,
				_startingLanternRange
			);
		}

		private void UpdateMenus(float delta)
		{
			UpdateStatMenu(delta);
		}

		private void UpdateStatMenu(float delta)
		{
			StatMenu.UpdateStats(Stats);
		}

		private void UpdateToxic(float delta)
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

		#endregion
	}
}
