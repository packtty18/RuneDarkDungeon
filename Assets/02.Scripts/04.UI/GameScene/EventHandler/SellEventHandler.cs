using UnityEngine;

public class SellEventHandler : ISlotEventHandler
{
    private IInventory _inventory;
    private ICurrency _currency;
    private InventoryManager _inventoryManager;

    public SellEventHandler(IInventory inventory, ICurrency currency, InventoryManager inventoryManager)
    {
        _inventory = inventory;
        _currency = currency;
        _inventoryManager = inventoryManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        _inventory.Remove(slot.Item);
        _currency.Add(100);
        Debug.Log("골드 획득");
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        _inventoryManager.ShowTooltip(slot, true);
    }

    public void OnDoubleClickSlot(UI_Slot slot) { }

    public void OnEnter()
    {
        _inventoryManager.SetSellCursor();
    }

    public void OnExit()
    {
        _inventoryManager.ResetCursor();
    }
}
