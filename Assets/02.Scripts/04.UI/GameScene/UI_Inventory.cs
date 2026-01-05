using System;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private void Start()
    {
        Refresh();
        InventoryManager.Instance.Inventory.Subscribe(AddSlot);
    }

    private void OnDestroy()
    {
        if (!InventoryManager.IsExist()) return;
        InventoryManager.Instance.Inventory.Unsubscribe(AddSlot);
    }

    // Todo: UI Manager를 통해 주입받는 방식으로 변경
    private void Refresh()
    {
        IReadOnlyInventory inventory = InventoryManager.Instance.Inventory;
        foreach (var item in inventory.Items)
        {
            AddSlot(item);
        }
    }

    private void AddSlot(ItemData itemData)
    {
        ItemSO itemInfo = DataManager.Instance.GetItemInfo(itemData.ID);
        Debug.Log($"룬 추가 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
