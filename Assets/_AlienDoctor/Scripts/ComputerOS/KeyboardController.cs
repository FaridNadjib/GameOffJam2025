using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private ScreenDisplay screenDisplay;
    [SerializeField] private LayerMask keyboardLayerMask;
    private void OnEnable()
    {
        KeyboardKey.OnKeyPressed += HandleKeyPressed;
    }

    private void OnDisable()
    {
        KeyboardKey.OnKeyPressed -= HandleKeyPressed;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
       
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, keyboardLayerMask))
        {
            // --- Keyboard Key click ---
            if (((1 << hit.collider.gameObject.layer) & keyboardLayerMask) != 0)
            {
                KeyboardKey key = hit.collider.GetComponentInParent<KeyboardKey>();
                if (key != null)
                {
                    key.Press();
                    return;
                }
            }
        }
    }
    private void HandleKeyPressed(string keyValue)
    {
        // Forward key input to the screen display
        screenDisplay?.ReceiveKeyInput(keyValue);
    }
}
