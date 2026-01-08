using System.Collections.Generic;
using UnityEngine;

public class SlotEventHandler : MonoBehaviour
{
    private ISlotEventHandler _eventHandler;
    
    private NormalEventHandler _normalEventHandler;
    private UpgradeEventHandler _upgradeEventHandler;

    private List<UI_Slot> _slots;

    public void Initialize(UpgradeManager upgradeManager, List<UI_Slot> slots, UI_Tooltip tooltip, UI_DragIcon dragIcon, UI_Background[] backgrounds)
    {
        _normalEventHandler = new(tooltip, dragIcon, backgrounds);
        _upgradeEventHandler = new(upgradeManager);
        _eventHandler = _normalEventHandler;
        
        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
            slot.OnSlotHovered -= OnHoverSlot;
        }
    }

    private void SetInventoryMode()
    {
        _eventHandler.OnExit();
        _eventHandler = _normalEventHandler;
        _eventHandler.OnEnter();
    }
    
    private void SetUpgradeMode()
    {
        _eventHandler.OnExit();
        _eventHandler = _upgradeEventHandler;
        _eventHandler.OnEnter();
    }

    private void OnClickSlot(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }

    private void OnHoverSlot(UI_Slot slot)
    {
        _eventHandler.OnHoverSlot(slot);
    }
}
