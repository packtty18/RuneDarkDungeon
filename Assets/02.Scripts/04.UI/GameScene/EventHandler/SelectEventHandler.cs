using UnityEngine;

public class SelectEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly InventoryManager _inventoryManager;

    public SelectEventHandler(IInventory inventory, InventoryManager inventoryManager)
    {
        _inventory = inventory;
        _inventoryManager = inventoryManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        var selectedItem = _inventoryManager.SelectedItem;
        
        if (selectedItem == null)
        {
            if (slot.IsEmpty) return;
            _inventoryManager.SelectItem(slot.Item);
        }
        else
        {
            _inventory.Swap(selectedItem, slot.Item);
            _inventoryManager.DeselectItem();
        }
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        _inventoryManager.ShowTooltip(slot);
    }
    
    public void OnDoubleClickSlot(UI_Slot slot) { }

    public void OnEnter() { }

    public void OnExit()
    {
        _inventoryManager.DeselectItem();
    }
}
