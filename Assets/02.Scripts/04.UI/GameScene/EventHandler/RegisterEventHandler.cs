using UnityEngine;

public class RegisterEventHandler : ISlotEventHandler
{
    private readonly IInventory _inventory;
    private readonly IForge _forge;
    
    public RegisterEventHandler(IInventory inventory, IForge forge)
    {
        _inventory = inventory;
        _forge = forge;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        if (!_forge.TryRegister(slot.Item)) return;
        _inventory.Remove(slot.Item);
        _forge.Notify();
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnDoubleClickSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit()
    {
        foreach (var item in _forge.Items)
        {
            _inventory.Add(item);
        }
        _forge.UnregisterAll();
    }
}
