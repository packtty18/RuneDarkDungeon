using System.Collections.Generic;
using UnityEngine;

public class InventoryEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_WindowToggleButton _upgradeUIButton;
    
    private ISlotEventHandler _eventHandler;
    private IReadOnlyList<UI_Slot> _slots;

    private UpgradeManager _upgradeManager;

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyList<UI_Slot> slots)
    {
        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
            slot.OnSlotHovered += OnHoverSlot;
        }
        SetMode(false);
        _upgradeUIButton.OnUpgradeMode += SetMode;
        
        _upgradeManager = upgradeManager;
        _upgradeManager.OnTargetTypeChanged += RefreshSlotState;
    }

    private void OnDestroy()
    {
        if (_slots == null) return;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
            slot.OnSlotHovered -= OnHoverSlot;
        }
        _upgradeUIButton.OnUpgradeMode -= SetMode;
        _upgradeManager.OnTargetTypeChanged -= RefreshSlotState;
    }

    private void SetMode(bool isUpgrade)
    {
        _eventHandler?.OnExit();
        _eventHandler = isUpgrade ? _registerEventHandler : _swapEventHandler;
        _eventHandler.OnEnter();

        RefreshSlotState(null);
    }

    private void RefreshSlotState(ItemData targetItem)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = _eventHandler.IsInteractable(slot, targetItem);
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
