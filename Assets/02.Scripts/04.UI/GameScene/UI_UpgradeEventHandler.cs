using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradeEventHandler : MonoBehaviour
{
    [SerializeField] private UI_Upgrade _upgrade;
    private UpgradeManager _upgradeManager;
    
    private List<UI_Slot> _inventorySlots;
    private List<UI_Slot> _upgradeSlots;
    private UI_Slot _resultSlot;
    
    public void Initialize(UpgradeManager upgradeManager, List <UI_Slot> inventorySlots, List<UI_Slot> upgradeSlots)
    {
        _upgradeManager = upgradeManager;
        _inventorySlots = inventorySlots;
        _upgradeSlots = upgradeSlots;
        Bind();
    }

    private void Bind()
    {
        foreach (var slot in _inventorySlots)
        {
            slot.OnSlotClicked += OnClickInventorySlot;
        }
        
        foreach (var slot in _upgradeSlots)
        {
            slot.OnSlotClicked += OnClickUpgradeSlot;
        }
        
        _upgrade.OnUIActived += UpgradeMode;
    }
    
    private void OnDestroy()
    {
        if (_inventorySlots != null)
        {
            foreach (var slot in _inventorySlots)
            {
                slot.OnSlotClicked -= OnClickInventorySlot;
            }
        }

        if (_upgradeSlots != null)
        {
            foreach (var slot in _upgradeSlots)
            {
                slot.OnSlotClicked -= OnClickUpgradeSlot;
            }
        }
        
        _upgrade.OnUIActived -= UpgradeMode;
    }
    
    public void UpgradeMode(bool isOn)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            slot.SetUpgradeMode(isOn);
        }
    }

    private void OnClickInventorySlot(UI_Slot slot)
    {
        _upgradeManager.TryRegister(slot.Data.Item);
    }

    private void OnClickUpgradeSlot(UI_Slot slot)
    {
        _upgradeManager.Unregister(slot.Data.Item);
    }
}
