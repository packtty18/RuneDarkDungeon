using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeEventHandler : MonoBehaviour
{
    private ISlotEventHandler _eventHandler;
    private List<UI_Slot> _slots;
    
    public void Initialize(UpgradeManager upgradeManager, List<UI_Slot>slots, TextMeshProUGUI costText, TextMeshProUGUI rateText)
    {
        _eventHandler = new UnregisterEventHandler(upgradeManager, slots, costText, rateText);

        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
        }
    }
    
    private void OnDestroy()
    {
        if (_slots == null) return;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
        }
    }

    private void OnClickSlot(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }
}
