using UnityEngine;

public class NormalEventHandler : ISlotEventHandler
{
    private InventoryManager _inventoryManager;

    public NormalEventHandler(InventoryManager inventoryManager)
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
