using System.Collections.Generic;
using UnityEngine;

public class UI_SlotContainer : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("슬롯 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    public IReadOnlyList<UI_Slot> Slots => _slots;

    private Dictionary<ItemData, UI_Slot> _itemSlotDict = new();
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        BindInventory();
    }

    private void BindInventory()
    {
        _itemSlotDict.Clear();
        foreach (var item in _inventory.Items)
        {
            SetSlot(item);
        }
        _inventory.Subscribe(SetSlot, ClearSlot);
    }
    
    private void OnDestroy()
    {
        _inventory?.Unsubscribe(SetSlot, ClearSlot);
    }
    
    private void SetSlot(ItemData itemData)
    {
        var data = _itemDB.GetSlotData(itemData);

        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(data);
            _itemSlotDict.Add(itemData, slot);
            return;
        }
    }
    
    private void ClearSlot(ItemData itemData)
    {
        if (!_itemSlotDict.TryGetValue(itemData, out var slot)) return;

        slot.Clear();
        _itemSlotDict.Remove(itemData);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
