using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class OutlineHover : MonoBehaviour
{
    [Header("Outline Settings")]
    [SerializeField] private int materialIndex = 1; // 0 = first, 1 = second, etc.
    [SerializeField] private string outlineProperty = "_outline_size";
    [SerializeField] private float hoverOutlineValue = 1.3f;
    [SerializeField] private float transitionTime = 0.25f;

    private Material _material;
    private Tween _tween;
    private float _currentOutline;

    void Start()
    {
        var renderer = GetComponent<Renderer>();

        // Make sure the index is valid
        if (renderer.materials.Length <= materialIndex)
        {
            Debug.LogError($"Material index {materialIndex} is out of range on {name}. " +
                           $"This object has {renderer.materials.Length} materials.");
            return;
        }

        // Get the correct material instance
        _material = renderer.materials[materialIndex];

        // Check the property
        if (_material.HasProperty(outlineProperty))
        {
            _currentOutline = _material.GetFloat(outlineProperty);
        }
        else
        {
            Debug.LogWarning($"Material '{_material.name}' does not have a float property '{outlineProperty}'.");
        }
    }

    void OnMouseEnter()
    {
        // ✅ Only trigger hover effect when camera is OMNIPOTENT
        if (CameraNavigationManager.Instance == null ||
            CameraNavigationManager.Instance.CurrentState != CameraNavigationManager.CameraState.OMNIPOTENT)
            return;

        SetOutline(hoverOutlineValue);
    }

    void OnMouseExit()
    {
        // Always reset to zero on exit — but optional: you can also gate this if needed
        SetOutline(0f);
    }

    private void SetOutline(float targetValue)
    {
        if (_material == null || !_material.HasProperty(outlineProperty)) return;

        _tween?.Kill();

        _tween = DOTween.To(
            () => _currentOutline,
            x => {
                _currentOutline = x;
                _material.SetFloat(outlineProperty, _currentOutline);
            },
            targetValue,
            transitionTime
        );
    }
}
