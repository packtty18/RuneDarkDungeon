using UnityEngine;

public class SwapEventHandler : ISlotEventHandler
{
    private InventoryManager _inventoryManager;

    public SwapEventHandler(InventoryManager inventoryManager)
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

    public void OnEnter() { }

    public void OnExit()
    {
        _inventoryManager.DeselectItem();
    }
}
