using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineMouseLookOffset : MonoBehaviour
{
    [Header("Look Settings")]
    public float maxAngle = 10f; // Max degrees left/right/up/down
    public float sensitivity = 1f;
    public float smoothTime = 0.1f;

    private CinemachineCamera vcam;
    private Quaternion initialRotation;
    private Vector2 currentOffset;
    private Vector2 targetOffset;
    private Vector2 velocity;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        initialRotation = transform.localRotation;
        Cursor.lockState = CursorLockMode.Confined; // optional
    }

    void Update()
    {
        // Get mouse delta
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Target offset (clamped)
        targetOffset.x += mouseX * sensitivity;
        targetOffset.y -= mouseY * sensitivity;
        targetOffset.x = Mathf.Clamp(targetOffset.x, -maxAngle, maxAngle);
        targetOffset.y = Mathf.Clamp(targetOffset.y, -maxAngle, maxAngle);

        // Smooth transition
        currentOffset.x = Mathf.SmoothDamp(currentOffset.x, targetOffset.x, ref velocity.x, smoothTime);
        currentOffset.y = Mathf.SmoothDamp(currentOffset.y, targetOffset.y, ref velocity.y, smoothTime);

        // Apply offset rotation (relative to initial)
        Quaternion yaw = Quaternion.AngleAxis(currentOffset.x, Vector3.up);
        Quaternion pitch = Quaternion.AngleAxis(currentOffset.y, Vector3.right);
        transform.localRotation = initialRotation * yaw * pitch;
    }

    void OnDisable()
    {
        // Reset rotation when camera deactivates
        transform.localRotation = initialRotation;
    }
}
