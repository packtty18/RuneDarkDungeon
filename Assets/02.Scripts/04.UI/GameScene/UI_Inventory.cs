using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    
    private void Start()
    {
        _inventory = InventoryManager.Instance.Inventory;
        Refresh();
        _inventory.Subscribe(AddSlot);
    }

    private void OnDestroy()
    {
        _inventory?.Unsubscribe(AddSlot);
    }

    private void Refresh()
    {
        foreach (var item in _inventory.Items)
        {
            AddSlot(item);
        }
    }

    private void AddSlot(ItemData itemData)
    {
        ItemSO itemInfo = InventoryManager.Instance.GetItemInfo(itemData.ID);
        Debug.Log($"룬 추가 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
