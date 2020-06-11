using UnityEngine;

public class Billboard : MonoBehaviour
{
    void Start()
    {
        
    }

    void LateUpdate()
    {
        var cameraForward = Camera.main.transform.forward;
        transform.forward = new Vector3(cameraForward.x, 0f, cameraForward.z);
    }
}
