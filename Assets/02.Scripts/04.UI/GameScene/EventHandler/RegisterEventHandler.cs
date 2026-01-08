using UnityEngine;

public class RegisterEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    
    public RegisterEventHandler(UpgradeManager upgradeManager)
    {
        _upgradeManager = upgradeManager;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.TryRegister(slot.Item);
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }

    public bool IsInteractable(UI_Slot slot, ItemData item)
    {
        return slot.CanUpgrade(item);
    }
}
