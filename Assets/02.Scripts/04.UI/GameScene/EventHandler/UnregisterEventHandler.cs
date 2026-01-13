using UnityEngine;

public class UnregisterEventHandler : ISlotEventHandler
{
    private readonly IForge _forge;
    
    public UnregisterEventHandler(IForge forge)
    {
        _forge = forge;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _forge.Unregister(slot.Item);
    }

    public void OnHoverSlot(UI_Slot slot) { }
    public void OnDoubleClickSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
