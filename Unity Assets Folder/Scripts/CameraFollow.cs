using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10); // Set your desired offset here
    public float downAngle = 40f; // Down-facing angle

    void FixedUpdate()
    {
        // Rotate the offset by the target's Y rotation
        Quaternion rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Instantly move the camera to the desired position (no smoothing)
        transform.position = desiredPosition;

        // Set the camera's rotation to the fixed down-facing angle, matching target's Y
        transform.rotation = Quaternion.Euler(downAngle, target.eulerAngles.y, 0);
    }

}