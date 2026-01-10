using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SlotFilter : UI_Base
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
    private IReadOnlyList<UI_Slot> _inventorySlots;
    private IReadOnlyList<UI_Slot> _upgradeSlots;
    
    private UpgradeManager _upgradeManager;
    private IReadOnlyInventory _upgradeInventory;

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyInventory upgradeInventory, IReadOnlyList<UI_Slot> inventorySlots, IReadOnlyList<UI_Slot> upgradeSlots)
    {
        _upgradeManager = upgradeManager;
        _upgradeInventory = upgradeInventory;
        
        _inventorySlots = inventorySlots;
        _upgradeSlots = upgradeSlots;
        
        upgradeManager.OnChanged.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _upgradeManager.OnChanged.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        RefreshSlotState();
        SetUpgradeInfo();
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
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            slot.SetInteractable(true);
        }
    }
    
    private void RefreshSlotState()
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = _upgradeInventory.CanAdd(slot.Item);
            slot.SetInteractable(isOn);
        }
    }
    
    private void SetUpgradeInfo()
    {
        var data = _upgradeManager.UpgradeData;
        
        SetUpgradeSlotCount(data.Count);
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
    }
    
    private void SetUpgradeSlotCount(int count)
    {
        foreach (var slot in _upgradeSlots)
        {
            slot.SetActive(count-- > 0);
        }
    }
}
