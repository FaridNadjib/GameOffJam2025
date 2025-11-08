using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class POVPoint : MonoBehaviour
{
    [Tooltip("The Cinemachine virtual camera for this point of view.")]
    public CinemachineCamera vCam;

    [Tooltip("Optional name for editor/UX.")]
    public string povName = "POV";

    [Tooltip("Optional event fired when this POV is entered.")]
    public UnityEvent onEnter;

    [Tooltip("Optional event fired when this POV is exited.")]
    public UnityEvent onExit;

    [HideInInspector]
    public bool isActive = true; // used by manager to enable/disable clicking

    [Header("Sub Cameras (optional)")]
    public List<CinemachineCamera> subCameras = new List<CinemachineCamera>();

    private int activeSubIndex = -1;

    void Reset()
    {
        // Try to find a CinemachineVirtualCamera on children by default
        vCam = GetComponentInChildren<CinemachineCamera>();
    }

    public void Enter()
    {
        // Optional: activate a default sub camera or enable scripts
        EnableExtraScripts(true);
    }

    public void Exit()
    {
        // Optional: disable all sub-camera scripts
        EnableExtraScripts(false);
        DeactivateAllSubCameras();
    }

    public void ActivateSubCamera(int index, int activePriority = 90, int idlePriority = 10)
    {
        if (index < 0 || index >= subCameras.Count) return;

        // Lower main camera priority
        if (vCam != null) vCam.Priority = idlePriority;

        // Lower old subcam if one was active
        if (activeSubIndex >= 0 && activeSubIndex < subCameras.Count)
            subCameras[activeSubIndex].Priority = idlePriority;

        // Activate new subcam
        subCameras[index].Priority = activePriority;
        activeSubIndex = index;
    }

    public void DeactivateAllSubCameras()
    {
        foreach (var cam in subCameras)
            if (cam != null) cam.Priority = 0;
        activeSubIndex = -1;
    }

    private void EnableExtraScripts(bool enable)
    {
        foreach (var cam in subCameras)
        {
            foreach (var comp in cam.GetComponents<MonoBehaviour>())
            {
                if (!(comp is CinemachineCamera))
                    comp.enabled = enable;
            }
        }
    }
}
