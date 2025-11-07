using UnityEngine;

public class LeftPanelItem : MonoBehaviour
{
    public GameObject unselected;
    public GameObject selected;
    
    public void SetSelect(bool value)
    {
        selected.SetActive(value);
        unselected.SetActive(!value);
    }
}
