using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Player Components")]
    [Tooltip("Character controller used for movement.")]
    public CharacterController Character;

    [Tooltip("Camera representing the player viewpoint.")]
    public PlayerCameraController FirstPersonCamera;

    [Header("Movement Settings")]
    [Tooltip("Multiplicative movement factor when sprinting.")]
    public float SprintFactor = 2.0f;
    
    [Tooltip("Base movement velocity.")]
    public float BaseVelocity = 10.0f;

    [Tooltip("Gravity acceleration.")]
    public Vector3 Gravity = new Vector3(0f, -9.8f, 0f);

    public Vector3 Velocity => Character.velocity;

    // Start is called before the first frame update
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Start()
    {
        
    }

    // Update is called once per frame
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Update()
    {
        // Translation
        var translation = GetInputTranslationDirection() * BaseVelocity * Time.deltaTime;

        // Speed up movement when shift key held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            translation *= SprintFactor;
        }

        // Only yaw is used, so that way the player does not move in whatever direction they face upwards or downwards.
        translation = Quaternion.Euler(0f, FirstPersonCamera.Yaw, 0f) * translation;

        translation += Gravity * Time.deltaTime;

        Character.Move(translation);
    }

    protected static Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        return direction;
    }
}
