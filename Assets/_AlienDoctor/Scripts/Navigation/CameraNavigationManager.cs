using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
/// <summary>
/// Handles switching between an Omnipotent (overview) camera and multiple POV cameras.
/// Singleton-based for global access.
/// </summary>
public class CameraNavigationManager : MonoBehaviour
{
    // ----------- Singleton -----------
    public static CameraNavigationManager Instance { get; private set; }

    // ----------- Enum for state -----------
    public enum CameraState
    {
        OMNIPOTENT,
        POVCAM
    }

    [Header("Cameras")]
    [Tooltip("Omnipotent (default) virtual camera")]
    public CinemachineCamera omnipotentVCam;

    [Tooltip("All POV points (auto-filled if empty)")]
    public List<POVPoint> povPoints = new List<POVPoint>();

    [Header("Raycast")]
    [Tooltip("Layers considered clickable for POVs")]
    public LayerMask clickLayerMask = ~0;

    [Header("Priority settings")]
    [Tooltip("Priority used when a VCam is 'active'")]
    public int activePriority = 50;
    [Tooltip("Priority used for omnipotent when it's not active")]
    public int omniIdlePriority = 10;
    [Tooltip("Default priority for all POVs when idle")]
    public int povIdlePriority = 0;

    // ----------- Private variables -----------
    private POVPoint currentPOV = null;
    private Camera mainCamera;
    private CameraState currentState = CameraState.OMNIPOTENT;

    public CameraState CurrentState => currentState;

    // ----------- Lifecycle -----------
    private void Awake()
    {
        // Singleton logic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        mainCamera = Camera.main;

        if (omnipotentVCam == null)
            Debug.LogError("Omnipotent vcam not assigned on CameraNavigationManager.");

        if (povPoints.Count == 0)
        {
            // Auto-discover POVPoints in scene
            var list = FindObjectsOfType<POVPoint>();
            povPoints = new List<POVPoint>(list);
        }

        InitializePriorities();
    }

    private void InitializePriorities()
    {
        if (omnipotentVCam != null)
            omnipotentVCam.Priority = activePriority;

        foreach (var p in povPoints)
        {
            if (p != null && p.vCam != null)
                p.vCam.Priority = povIdlePriority;
        }

        currentState = CameraState.OMNIPOTENT;
    }

    private void Update()
    {
        switch (currentState)
        {
            case CameraState.OMNIPOTENT:
                if (Input.GetMouseButtonDown(0))
                    HandleClick();
                break;

            case CameraState.POVCAM:
                if (Input.GetKeyDown(KeyCode.Escape))
                    ReturnToOmnipotent();
                break;
        }
    }

    // ----------- Core Logic -----------
    private void HandleClick()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, clickLayerMask))
        {
            POVPoint pov = hit.collider.GetComponentInParent<POVPoint>();
            if (pov != null && pov.isActive)
            {
                EnterPOV(pov);
            }
        }
    }

    public void EnterPOV(POVPoint pov)
    {
        if (pov == null || pov.vCam == null) return;

        // Switch state
        currentState = CameraState.POVCAM;

        // Lower omnipotent camera priority
        if (omnipotentVCam != null)
            omnipotentVCam.Priority = omniIdlePriority;

        // Raise POV camera priority
        pov.vCam.Priority = activePriority;
        currentPOV = pov;
        pov.Enter();

        // Disable other POV clicks
        SetPOVsClickable(false);
        pov.isActive = true;
    }

    public void ReturnToOmnipotent()
    {
        if (currentPOV != null)
        {
            if (currentPOV.vCam != null)
                currentPOV.vCam.Priority = povIdlePriority;

            currentPOV.Exit();
            currentPOV = null;
        }

        if (omnipotentVCam != null)
            omnipotentVCam.Priority = activePriority;

        SetPOVsClickable(true);
        currentState = CameraState.OMNIPOTENT;
    }

    private void SetPOVsClickable(bool clickable)
    {
        foreach (var p in povPoints)
        {
            if (p == null) continue;
            p.isActive = clickable;
        }
    }

    // ----------- Public utility methods -----------
    public void EnterPOVByName(string name)
    {
        var pov = povPoints.Find(x => x != null && x.povName == name);
        if (pov != null)
            EnterPOV(pov);
        else
            Debug.LogWarning($"No POV named '{name}' found!");
    }

    public bool IsOmnipotent => currentState == CameraState.OMNIPOTENT;
    public bool IsInPOV => currentState == CameraState.POVCAM;
}
