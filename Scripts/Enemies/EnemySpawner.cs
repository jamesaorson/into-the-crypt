using System;
using Godot;
using IntoTheCrypt.Enemies.Controllers;

namespace IntoTheCrypt.Enemies
{
	public class EnemySpawner : Spatial
	{
		#region Public

		#region Member Methods
		public override void _Ready()
		{
			LoadScenes();
			foreach (Node child in GetChildren())
			{
				if (child == null)
				{
					continue;
				}
				var enemy = InstantiateEnemy(_squogScene);
				enemy.Translation = Vector3.Zero;
				child.AddChild(enemy);
			}
		}
		#endregion

		#endregion

		#region Protected

		#region Members
		public PackedScene _squogScene;
		#endregion

		#region Member Methods
		private EnemyController InstantiateEnemy(PackedScene enemyScene)
		{
			var enemy = enemyScene.Instance() as EnemyController;
			if (enemy == null)
			{
				throw new Exception($"Expected enemy to be an EnemyController node");
			}
			return enemy;
		}
	
		protected void LoadScenes()
		{
			_squogScene = GD.Load<PackedScene>(Constants.ResourceSquog);
			if (_squogScene == null)
			{
				throw new Exception("Squog scene did not load correctly");
			}
		}
		#endregion

		#endregion
	}
}
