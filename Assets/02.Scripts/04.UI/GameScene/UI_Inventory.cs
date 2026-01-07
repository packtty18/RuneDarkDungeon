using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    
    [Header("UI 연결")]
    [SerializeField] private List<UI_Slot> _slots;
    [SerializeField] private UI_DragIcon _dragIcon;
    
    private UI_Slot _selectedSlot;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        Refresh();
        BindEvents();
    }
    
    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= ClickSlot;   
        }
        _inventory?.Unsubscribe(SetSlot);
    }

    private void Refresh()
    {
        foreach (var item in _inventory.Items)
        {
            SetSlot(item);
        }
    }

    private void BindEvents()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += ClickSlot;   
        }
        _inventory.Subscribe(SetSlot);
    }

    private void SetSlot(ItemData itemData)
    {
        ItemSO itemInfo = _itemDB.GetItemInfo(itemData.ID);
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(itemData, itemInfo);
            return;
        }
    }
    
    private void SwapSlot(UI_Slot left, UI_Slot right)
    {
        UI_Slot slot = left;
        left.SetItem(right.Item, right.Info);
        right.SetItem(slot.Item, slot.Info);
    }
    
    private void ClearSlot(UI_Slot slot)
    {
        slot.Clear();   
    }
    
    private void ClickSlot(UI_Slot slot)
    {
        if (_selectedSlot == null)
        {
            if (slot.IsEmpty) return;
            _selectedSlot = slot;
            _dragIcon.Show(slot.Info);
        }
        else
        {
            SwapSlot(_selectedSlot, slot);
            _selectedSlot = null;
            _dragIcon.Hide();
        }
    }
}
