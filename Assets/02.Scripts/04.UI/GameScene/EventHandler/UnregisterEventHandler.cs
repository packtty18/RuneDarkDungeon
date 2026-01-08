using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnregisterEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    private UI_Slot _resultSlot;
    
    public UnregisterEventHandler(UpgradeManager upgradeManager)
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
