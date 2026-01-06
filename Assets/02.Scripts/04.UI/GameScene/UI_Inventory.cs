using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [SerializeField] private List<Slot> _slots;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        
    }

    private void Start()
    {
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
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(itemData, itemInfo);
            return;
        }
    }
}
