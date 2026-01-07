using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private IReadOnlyInventory _inventory;
    private ItemDatabaseSO _itemDB;
    private GradeColorSO _colorDB;
    
    [Header("UI 연결")]
    [Space]
    [SerializeField] private List<UI_Slot> _slots;
    [Space]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    
    private UI_Slot _selectedSlot;
    
    public void Initialize(IReadOnlyInventory inventory, ItemDatabaseSO itemDB, GradeColorSO colorDB)
    {
        _inventory = inventory;
        _itemDB = itemDB;
        _colorDB = colorDB;
        Refresh();
        BindEvents();
    }
    
    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;   
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
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
        _inventory.Subscribe(SetSlot);
    }

    private void SetSlot(ItemData itemData)
    {
        ItemSO itemInfo = _itemDB.GetItemInfo(itemData.ID);
        Color color = _colorDB.GetColor(itemData.Grade);

        SlotData data = new(itemData, itemInfo, color);
        
        foreach (var slot in _slots)
        {
            if (!slot.IsEmpty) continue;
            slot.SetItem(data);
            return;
        }
    }
    
    private void SwapSlot(UI_Slot left, UI_Slot right)
    {
        SlotData data = left.Data;
        left.SetItem(right.Data);
        right.SetItem(data);
    }
    
    private void OnClickSlot(UI_Slot slot)
    {
        if (_selectedSlot == null)
        {
            if (slot.IsEmpty) return;
            _selectedSlot = slot;
            _dragIcon.Show(slot.Icon);
        }
        else
        {
            SwapSlot(_selectedSlot, slot);
            _selectedSlot = null;
            _dragIcon.Hide();
        }
    }

    private void OnHoverSlot(UI_Slot slot)
    {
        if (slot == null)
        {
            _tooltip.Hide();
            return;
        }
        _tooltip.Show(slot.Data.Info, slot.transform);
    }
}
