using System.Collections.Generic;
using UnityEngine;

public class InventoryEventHandler : MonoBehaviour
{
    private ISlotEventHandler _eventHandler;
    
    private SwapEventHandler _swapEventHandler;
    private RegisterEventHandler _registerEventHandler;

    private List<UI_Slot> _slots;

    public void Initialize(UpgradeManager upgradeManager, List<UI_Slot> slots, UI_Tooltip tooltip, UI_DragIcon dragIcon, UI_Background[] backgrounds)
    {
        _swapEventHandler = new(tooltip, dragIcon, backgrounds);
        _registerEventHandler = new(upgradeManager);
        SetMode(false);

        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
    }

    private void OnDestroy()
    {
        if (_slots == null) return;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
            slot.OnSlotHovered -= OnHoverSlot;
        }
    }

    public void SetMode(bool isUpgrade)
    {
        _eventHandler?.OnExit();
        _eventHandler = isUpgrade ? _registerEventHandler : _swapEventHandler;
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
