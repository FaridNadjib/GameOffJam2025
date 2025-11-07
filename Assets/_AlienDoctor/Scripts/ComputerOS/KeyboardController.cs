using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private ScreenDisplay screenDisplay;

    private void OnEnable()
    {
        KeyboardKey.OnKeyPressed += HandleKeyPressed;
    }

    private void OnDisable()
    {
        KeyboardKey.OnKeyPressed -= HandleKeyPressed;
    }

    private void HandleKeyPressed(string keyValue)
    {
        // Forward key input to the screen display
        screenDisplay?.ReceiveKeyInput(keyValue);
    }
}
