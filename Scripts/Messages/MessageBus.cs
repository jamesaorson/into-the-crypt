using Godot;

namespace IntoTheCrypt.Messages
{
	public class MessageBus : Node
	{
		[Signal]
		public delegate void EnemyDeath(ulong instanceId);
		[Signal]
		public delegate void EnemyAttack(DamagePlayerMessage damage);
	}
}
