using System;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private void Start()
    {
        Refresh();
        DataManager.Instance.SubscribeRune(AddSlot);
    }

    private void OnDestroy()
    {
        if (DataManager.Instance == null) return;
        DataManager.Instance.UnsubscribeRune(AddSlot);
    }

    private void Refresh()
    {
        IReadOnlyInventory inventory = DataManager.Instance.Inventory;
        foreach (var rune in inventory.Runes)
        {
            AddSlot(rune);
        }
    }

    private void AddSlot(RuneData runeData)
    {
        RuneSO runeInfo = DataManager.Instance.GetRuneInfo(runeData.ID);
        Debug.Log($"룬 추가 [{runeInfo.Name}] : {runeInfo.Tooltip}");
    }
}
