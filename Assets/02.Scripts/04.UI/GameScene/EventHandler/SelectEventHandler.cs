using UnityEngine;

public class SelectEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly SelectionManager _selectionManager;

    public SelectEventHandler(IInventory inventory, SelectionManager selectionManager)
    {
        _inventory = inventory;
        _selectionManager = selectionManager;
    }

    public void OnClickSlot(UI_Slot slot)
    {
        var selectedItem = _selectionManager.SelectedItem;
        
        if (selectedItem == null)
        {
            if (slot.IsEmpty) return;
            _selectionManager.SelectItem(slot.Item);
            
            SoundManager.Instance?.Play(ESoundType.Rune);
        }
        else
        {
            _inventory.Swap(selectedItem, slot.Item);
            _selectionManager.DeselectItem();
            
            SoundManager.Instance?.Play(ESoundType.Rune);
        }
    }

    public void OnHoverSlot(UI_Slot slot)
    {
        _selectionManager.ShowTooltip(slot);
    }
    
    public void OnDoubleClickSlot(UI_Slot slot) { }

    public void OnEnter() { }

    public void OnExit()
    {
        _selectionManager.DeselectItem();
    }
}
