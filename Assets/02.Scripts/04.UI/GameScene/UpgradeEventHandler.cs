using UnityEngine;

public class UpgradeEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    
    public UpgradeEventHandler(UpgradeManager upgradeManager)
    {
        _upgradeManager = upgradeManager;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.TryRegister(slot.Data.Item);
    }
    
    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
