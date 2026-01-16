using UnityEngine;

public class SelectEventHandler : ISlotEventHandler
{
    private readonly InventoryManager _inventoryManager;

    public SelectEventHandler(InventoryManager inventoryManager)
    {
        _inventoryManager = inventoryManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        _inventoryManager.SelectItem(slot);
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
