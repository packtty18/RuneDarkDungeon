using UnityEngine;

public class UpgradeSlotEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    
    private UI_Slot _resultSlot;
    
    public UpgradeSlotEventHandler(UpgradeManager upgradeManager)
    {
        _upgradeManager = upgradeManager;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Unregister(slot.Data.Item);
    }

    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
