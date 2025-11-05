using UnityEngine;
using DG.Tweening;
using System;
using Unity.Cinemachine;

[RequireComponent(typeof(Collider))]
public class KeyboardKey : MonoBehaviour
{
    public string keyValue = "A";                 // What this key represents
    public AudioClip pressSound;                  // Optional sound
    public float pressDepth = 0.05f;              // How far it moves down
    public float pressDuration = 0.1f;            // Animation speed

    private Vector3 _originalPos;
    [SerializeField ]private AudioSource _audioSource;

    // Event for when this key is pressed
    public static event Action<string> OnKeyPressed;

    void Start()
    {
        _originalPos = transform.localPosition;
    }

    public void Press()
    {
        // Animate key down and up
        transform.DOLocalMoveY(_originalPos.y - pressDepth, pressDuration / 2)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                     transform.DOLocalMoveY(_originalPos.y, pressDuration / 2)
                              .SetEase(Ease.InQuad)
                 );

        // Play sound
        if (pressSound)
            _audioSource.PlayOneShot(pressSound);

        // Notify listeners (e.g., the screen)
        OnKeyPressed?.Invoke(keyValue);
    }

    // Example trigger (can be called via Raycast, OnMouseDown, etc.)
    private void OnMouseDown()
    {
        if (CameraNavigationManager.Instance.CurrentState != CameraNavigationManager.CameraState.POVCAM)
            return;
        Press();
    }
}
