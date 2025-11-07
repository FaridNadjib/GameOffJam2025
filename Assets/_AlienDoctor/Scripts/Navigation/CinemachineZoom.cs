using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;           // How fast to zoom
    public float smoothTime = 0.1f;        // How smooth to interpolate
    public float minFOV = 30f;             // Closest zoom
    public float maxFOV = 60f;             // Farthest zoom

    private CinemachineCamera vcam;
    private float targetFOV;
    private float fovVelocity;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        targetFOV = vcam.Lens.FieldOfView;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            targetFOV -= scroll * zoomSpeed;
            targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
        }

        // Smooth FOV transition
        float currentFOV = vcam.Lens.FieldOfView;
        float newFOV = Mathf.SmoothDamp(currentFOV, targetFOV, ref fovVelocity, smoothTime);
        vcam.Lens.FieldOfView = newFOV;
    }
}
