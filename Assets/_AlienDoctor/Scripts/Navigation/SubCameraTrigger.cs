using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SubCameraTrigger : MonoBehaviour
{
    [Tooltip("The parent POVPoint that owns the sub-cameras")]
    public POVPoint parentPOV;

    [Tooltip("Index of sub-camera inside the parent POVPoint")]
    public int subCameraIndex = 0;

    private void OnMouseDown()
    {
        // Only respond when inside the POV (not global)
        if (CameraNavigationManager.Instance == null) return;
        if (!CameraNavigationManager.Instance.IsInPOV) return;
        if (parentPOV == null) return;

        // Check that this trigger belongs to the currently active POV
        if (CameraNavigationManager.Instance.CurrentPOV != parentPOV) return;

        CameraNavigationManager.Instance.EnterSubCamera(subCameraIndex);
    }
}
