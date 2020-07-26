using Godot;

namespace IntoTheCrypt.Player.Controllers
{
	public class PlayerMoveController : KinematicBody
	{
		#region Public

		#region Members
		public PlayerCameraController FirstPersonCamera;

		[Export(PropertyHint.Range, "1,5,or_greater")]
		public float SprintFactor = 1f;
		[Export]
		public Vector3 Gravity = Vector3.Down;
		[Export(PropertyHint.Range, "1,10,or_greater")]
		public float GravityFactor = 1;

		[Export(PropertyHint.Range, "10,100,or_greater")]
		public float BaseVelocity = 10f;
		public PlayerCameraController PlayerCamera { get; private set; }
		#endregion

		#endregion

		#region Private

		#region Member Methods
		public override void _PhysicsProcess(float delta)
		{
			HandleInput(delta);
		}

		public override void _Ready()
		{
			PlayerCamera = GetNode<PlayerCameraController>("CameraContainer");
		}
		#endregion

		private void HandleInput(float delta)
		{
			var direction = Vector3.Zero;
			bool isSprinting = false;

			if (Input.IsActionPressed("forward"))
			{
				direction += Vector3.Forward;
			}
			if (Input.IsActionPressed("backward"))
			{
				direction += Vector3.Back;
			}
			if (Input.IsActionPressed("left"))
			{
				direction += Vector3.Left;
			}
			if (Input.IsActionPressed("right"))
			{
				direction += Vector3.Right;
			}
			if (Input.IsActionPressed("sprint"))
			{
				isSprinting = true;
			}
			direction = direction.Normalized();

			var basis = PlayerCamera.GlobalTransform.basis;
			//var movement = Gravity * GravityFactor;
			var movement = Vector3.Zero;
			movement += basis.x * direction.x;
			movement += basis.z * direction.z;

			MoveAndSlide(
				(
					movement
					* BaseVelocity
					* delta
					* (isSprinting ? SprintFactor : 1f)
				),
				Vector3.Up,
				false
			);
		}

		#endregion
	}
}
