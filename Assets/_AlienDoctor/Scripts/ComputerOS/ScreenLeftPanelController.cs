using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ScreenLeftPanelController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<LeftPanelItem> items;
    private int currentIndex = 0;
    void Start()
    {
        UpdateHighlight();
    }


    void UpdateHighlight()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetSelect(i == currentIndex);
        }
    }

    public void ProcessUp()
    {
        if (currentIndex == 0)
            return;
        currentIndex--;
        UpdateHighlight();
    }

    public void ProcessDown()
    {
        if (currentIndex >= items.Count -1)
            return;
        currentIndex++;
        UpdateHighlight();
    }

}
