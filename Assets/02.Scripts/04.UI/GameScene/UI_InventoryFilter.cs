using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryFilter : UI_Base
{
    private IReadOnlyList<UI_Slot> _slots;
    
    private UpgradeManager _upgradeManager;
    private IReadOnlyInventory _upgradeInventory;

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyInventory upgradeInventory, IReadOnlyList<UI_Slot> inventorySlots)
    {
        _upgradeManager = upgradeManager;
        _upgradeInventory = upgradeInventory;
        
        _slots = inventorySlots;
        
        _upgradeManager.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _upgradeManager.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        RefreshSlotState();
    }

    public override void Show()
    {
        RefreshSlotState();
    }

    public override void Hide()
    {
        ResetSlotState();
    }

    private void ResetSlotState()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            slot.SetInteractable(true);
        }
    }
    
    private void RefreshSlotState()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            bool isOn = _upgradeInventory.CanAdd(slot.Item);
            slot.SetInteractable(isOn);
            slot.SetActive(true);
        }
    }
}
