using System;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private void Start()
    {
        DataManager.Instance.SubscribeRune(AddUI);
    }

    private void OnDestroy()
    {
        if (DataManager.Instance == null) return;
        DataManager.Instance.UnsubscribeRune(AddUI);
    }

    private void AddUI(RuneData rune)
    {
        
    }
}
