using Godot;

namespace IntoTheCrypt.Player.Controllers
{
	public class PlayerMoveController : KinematicBody
	{
		#region Public

		#region Members
		public PlayerCameraController FirstPersonCamera;
		public PlayerCameraController PlayerCamera { get; private set; }
		#endregion
		
		#region Member Methods
		public override void _PhysicsProcess(float delta)
		{
			HandleInput(delta);
		}

		public override void _Ready()
		{
			PlayerCamera = GetNode<PlayerCameraController>("Controller/CameraContainer");
		}
		#endregion

		#endregion

		#region Private

		#region Members
		private bool IsMoving { get; set; }
		#endregion

		#region Member Methods
		private void HandleInput(float delta)
		{
			HandleMoveInput(delta);
		}

		private void HandleMoveInput(float delta)
		{
			if (IsMoving)
			{
				return;
			}
			var direction = Vector3.Zero;
			if (Input.IsActionJustPressed("forward"))
			{
				direction += Vector3.Forward;
			}
			else if (Input.IsActionJustPressed("backward"))
			{
				direction += Vector3.Back;
			}
			else if (Input.IsActionJustPressed("left"))
			{
				direction += Vector3.Left;
			}
			else if (Input.IsActionJustPressed("right"))
			{
				direction += Vector3.Right;
			}
			if (direction == Vector3.Zero)
			{
				IsMoving = false;
				return;
			}
			IsMoving = true;
			MakeMove(direction, delta);
		}

		private void MakeMove(Vector3 direction, float delta)
		{
			Translate(direction);
			IsMoving = false;
		}
		#endregion

		#endregion
	}
}
