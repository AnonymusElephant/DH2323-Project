using UnityEngine;

public class PointTowardsCamera : MonoBehaviour
{
    //Makes the plane y-axis always face the camera
    [SerializeField]
    private Camera mainCamera;
    public bool showGizmos = false; // Option to toggle Gizmos visibility
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Fallback to the main camera if not assigned
        }
    }
    private void Update()
    {
        if (mainCamera != null)
        {
            // Get the camera's positive Z axis (forward)
            Vector3 cameraForward = mainCamera.transform.forward;
            // Project onto the ground plane (XZ)
            // cameraForward.y = 0;
            if (cameraForward.sqrMagnitude > 0.0001f)
            {
                cameraForward.Normalize();
                // Set the GameObject's forward (Z+) to match the camera's forward (Z+), keeping it upright
                transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            }
        }
    }
    private void OnDrawGizmos()
    {
        // Draw a line from the object to the camera for visualization
        if (mainCamera != null && showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, mainCamera.transform.position);
        }
    }
}
