using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    public Camera targetCamera; // The camera to face, usually the main camera.

    void Start()
    {
        // If no camera is assigned, use the main camera
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        // Make the UI element face the camera
        transform.LookAt(transform.position + targetCamera.transform.rotation * Vector3.forward,
                         targetCamera.transform.rotation * Vector3.up);
    }
}
