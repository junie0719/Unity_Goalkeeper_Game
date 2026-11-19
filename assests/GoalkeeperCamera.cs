
using UnityEngine;

public class GoalkeeperCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0f, 3.5f, 6f);
    public float smoothSpeed = 8f;
    public float cameraAngle = 12f;

    void LateUpdate()
    {
        if (target == null) return;

        // Follow goalkeeper in all directions
        Vector3 desiredPosition = target.position + offset;

        float t = 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            t
        );

        // Keep camera facing toward the striker
        transform.rotation = Quaternion.Euler(
            cameraAngle,
            180f,
            0f
        );
    }
}
