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

    // Todo: UI Manager를 통해 주입받는 방식으로 변경
    private void Refresh()
    {
        IReadOnlyInventory inventory = DataManager.Instance.Inventory;
        foreach (var item in inventory.Items)
        {
            AddSlot(item);
        }
    }

    private void AddSlot(ItemData itemData)
    {
        ItemSO itemInfo = DataManager.Instance.GetRuneInfo(itemData.ID);
        Debug.Log($"룬 추가 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
