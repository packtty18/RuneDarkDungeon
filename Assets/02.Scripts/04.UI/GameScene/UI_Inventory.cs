using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    private void Awake()
    {
        _itemDB = DataManager.Instance.ItemDB;
        _inventory = DataManager.Instance.Inventory;
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
        ItemSO itemInfo = _itemDB.GetItemInfo(itemData.ID);
        Debug.Log($"룬 추가 [{itemInfo.Name}] : {itemInfo.Tooltip}");
    }
}
