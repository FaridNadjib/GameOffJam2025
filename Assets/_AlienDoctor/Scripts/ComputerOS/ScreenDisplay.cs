using UnityEngine;
using TMPro;

public class ScreenDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private ScreenLeftPanelController leftPanelController;
    private string currentInput = "";

    public void ReceiveKeyInput(string keyValue)
    {
        switch (keyValue)
        {
            case "Up":
                ProcessUpCommand();
                break;
            case "Down":
                ProcessDownCommand();
                break;
            case "Enter":
                ProcessCommand();
                break;
            case "Backspace":
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                break;
            default:
                currentInput += keyValue;
                break;
        }

        UpdateDisplay();
    }

    private void ProcessCommand()
    {
        // Handle command execution, or just clear input
        currentInput = "";
    }
    private void ProcessUpCommand()
    {
        leftPanelController.ProcessUp();
    }
    private void ProcessDownCommand()
    {
        leftPanelController.ProcessDown();
    }

    private void UpdateDisplay()
    {
        displayText.text = currentInput;
    }
}
