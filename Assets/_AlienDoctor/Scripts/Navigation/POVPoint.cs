using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;

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

    void Reset()
    {
        // Try to find a CinemachineVirtualCamera on children by default
        vCam = GetComponentInChildren<CinemachineCamera>();
    }

    /// <summary>
    /// Called by the manager when this POV is activated.
    /// </summary>
    public void Enter()
    {
        onEnter?.Invoke();
    }

    /// <summary>
    /// Called by the manager when this POV is deactivated/returned.
    /// </summary>
    public void Exit()
    {
        onExit?.Invoke();
    }
}
