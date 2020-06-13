using UnityEngine;

namespace IntoTheCrypt.Player.Controllers
{
    public class PlayerCameraController : MonoBehaviour
    {
        private class CameraState
        {
            public float Yaw;
            public float Pitch;
            public float Roll;
            /*public float X;
            public float Y;
            public float Z;*/

            public void SetFromTransform(Transform t)
            {
                Pitch = t.eulerAngles.x;
                Yaw = t.eulerAngles.y;
                Roll = t.eulerAngles.z;
                /*X = t.position.x;
                Y = t.position.y;
                Z = t.position.z;*/
            }

            public void LerpTowards(CameraState target, float rotationLerpPct)
            {
                Yaw = Mathf.Lerp(Yaw, target.Yaw, rotationLerpPct);
                Pitch = Mathf.Lerp(Pitch, target.Pitch, rotationLerpPct);
                Roll = Mathf.Lerp(Roll, target.Roll, rotationLerpPct);

                /*X = Mathf.Lerp(X, target.X, positionLerpPct);
                Y = Mathf.Lerp(Y, target.Y, positionLerpPct);
                Z = Mathf.Lerp(Z, target.Z, positionLerpPct);*/
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(Pitch, Yaw, Roll);
                /*t.position = new Vector3(X, Y, Z);*/
            }
        }

        private CameraState _targetCameraState = new CameraState();
        private CameraState _interpolatingCameraState = new CameraState();

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float PositionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve MouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float RotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool InvertY = false;

        [Tooltip("How far up and down the angle of the camera can go.")]
        public float TopVerticalAngleLimit = 90;
        public float BottomVerticalAngleLimit = 80;

        public float Yaw => _interpolatingCameraState.Yaw;
        public float Pitch => _interpolatingCameraState.Pitch;
        public float Roll => _interpolatingCameraState.Roll;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void OnEnable()
        {
            _targetCameraState.SetFromTransform(transform);
            _interpolatingCameraState.SetFromTransform(transform);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void Update()
        {
            // Exit Sample  
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            // Hide and lock cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Rotation
            var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (InvertY ? 1 : -1));

            var mouseSensitivityFactor = MouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

            _targetCameraState.Yaw += mouseMovement.x * mouseSensitivityFactor;
            _targetCameraState.Pitch += mouseMovement.y * mouseSensitivityFactor;

            _targetCameraState.Pitch = Mathf.Clamp(_targetCameraState.Pitch, -TopVerticalAngleLimit, BottomVerticalAngleLimit);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            /*var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / PositionLerpTime) * Time.deltaTime);*/
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / RotationLerpTime) * Time.deltaTime);
            _interpolatingCameraState.LerpTowards(_targetCameraState, rotationLerpPct);

            _interpolatingCameraState.UpdateTransform(transform);
        }
    }
}