using UnityEngine;

public class UnregisterEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    
    public UnregisterEventHandler(UpgradeManager upgradeManager)
    {
        _upgradeManager = upgradeManager;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Unregister(slot.Item);
    }

    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }

    public bool IsInteractable(UI_Slot slot, ItemData item)
    {
        return true;
    }
}
