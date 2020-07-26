using Godot;
using static Godot.Input;

namespace IntoTheCrypt.Player.Controllers
{
	public class PlayerCameraController : Spatial
	{
		#region Public

		#region Members
		[Export(PropertyHint.Range, "0.01,10")]
		public float Sensitivity = 0f;
		[Export]
		public float MaxAngle = 70f;
		[Export]
		public float MinAngle = -70;
		#endregion

		#region Member Methods
		public override void _Input(InputEvent @event)
		{
			if (Input.GetMouseMode() != MouseMode.Captured)
			{
				return;
			}
			switch (@event)
			{
				case InputEventMouseMotion mouseMotion:
					var movement = mouseMotion.Relative;
					Rotation = new Vector3(Rotation.x, Rotation.y, 0f);
					_camera.RotateX(Mathf.Deg2Rad(-movement.y * Sensitivity));
					RotateY(Mathf.Deg2Rad(-movement.x * Sensitivity));

					var rotation = _camera.RotationDegrees;
					rotation.x = Mathf.Clamp(rotation.x, MinAngle, MaxAngle);
					_camera.RotationDegrees = rotation;
					break;
				default:
					break;
			}
		}

		public override void _Ready()
		{
			_camera = GetNode<Camera>("Camera");
			Input.SetMouseMode(MouseMode.Captured);
		}
		#endregion

		#endregion

		#region Private

		#region Members
		private Camera _camera;
		#endregion

		#endregion
	}
}
