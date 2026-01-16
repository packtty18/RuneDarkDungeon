using UnityEngine;

public class SellEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly ICurrency _currency;
    private readonly InventoryManager _inventoryManager;

    public SellEventHandler(IInventory inventory, ICurrency currency, InventoryManager inventoryManager)
    {
        _inventory = inventory;
        _currency = currency;
        _inventoryManager = inventoryManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        _currency.Add(slot.Item.Info.Price);
        _inventory.Remove(slot.Item);
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
