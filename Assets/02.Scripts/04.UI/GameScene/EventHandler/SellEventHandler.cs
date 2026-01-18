using UnityEngine;

public class SellEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly ICurrency _currency;
    private readonly SelectionManager _selectionManager;

    public SellEventHandler(IInventory inventory, ICurrency currency, SelectionManager selectionManager)
    {
        _inventory = inventory;
        _currency = currency;
        _selectionManager = selectionManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        _currency.Add(slot.Item.Info.Price);
        _inventory.Remove(slot.Item);
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        _selectionManager.ShowTooltip(slot, true);
    }

    public void OnDoubleClickSlot(UI_Slot slot) { }

    public void OnEnter()
    {
        _selectionManager.SetSellCursor();
    }

    public void OnExit()
    {
        _selectionManager.ResetCursor();
    }
}
