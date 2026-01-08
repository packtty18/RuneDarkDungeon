using System.Collections.Generic;
using UnityEngine;

public class InventoryEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_Tooltip _tooltip;
    [SerializeField] private UI_DragIcon _dragIcon;
    [SerializeField] private UI_Background[] _backgrounds;
    [SerializeField] private UI_WindowToggleButton _upgradeUIButton;
    
    private ISlotEventHandler _eventHandler;
    
    private SwapEventHandler _swapEventHandler;
    private RegisterEventHandler _registerEventHandler;

    private List<UI_Slot> _slots;

    public void Initialize(UpgradeManager upgradeManager, List<UI_Slot> slots)
    {
        _swapEventHandler = new(_tooltip, _dragIcon, _backgrounds);
        _registerEventHandler = new(upgradeManager);

        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
        SetMode(false);
        _upgradeUIButton.OnUpgradeMode += SetMode;
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
        
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = !isUpgrade || slot.Item.Grade != EItemGrade.Legendary;
            slot.SetInteractable(isOn);
        }
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
