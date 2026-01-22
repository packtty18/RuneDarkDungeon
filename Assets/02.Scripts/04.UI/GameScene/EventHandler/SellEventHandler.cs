using UnityEngine;

public class SellEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly ICurrency _currency;
    private readonly SelectionManager _selectionManager;
    private readonly ItemPriceDataSO _priceDB;

    public SellEventHandler(IInventory inventory, ICurrency currency, SelectionManager selectionManager, ItemPriceDataSO priceDB)
    {
        _inventory = inventory;
        _currency = currency;
        _selectionManager = selectionManager;
        _priceDB = priceDB;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        SoundManager.Instance?.Play(ESoundType.Rune_Sell);

        int price = _priceDB.GetPrice(slot.Item);
        _currency.Add(price);
        _inventory.Remove(slot.Item);
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        if (slot == null)
        {
            _selectionManager.ShowTooltip(slot);
            return;
        }
        int price = _priceDB.GetPrice(slot.Item);
        _selectionManager.ShowTooltip(slot, price);
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
