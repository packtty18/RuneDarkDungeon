using UnityEngine;

public class RegisterEventHandler : ISlotEventHandler
{
    private readonly IForge _forge;
    
    public RegisterEventHandler(IForge forge)
    {
        _forge = forge;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _forge.Register(slot.Item);
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }

    public void OnExit()
    {
        _forge.UnregisterAll();
    }
}
