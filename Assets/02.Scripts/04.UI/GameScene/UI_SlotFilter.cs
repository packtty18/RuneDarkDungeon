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

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyList<UI_Slot> inventorySlots, IReadOnlyList<UI_Slot> upgradeSlots)
    {
        _upgradeManager = upgradeManager;
        
        _inventorySlots = inventorySlots;
        _upgradeSlots = upgradeSlots;
    }

    public override void Refresh()
    {
        // 강화 정보 바뀔 때
        RefreshSlotState(_upgradeManager.TargetType);
        SetUpgradeInfo();
    }

    public override void Show()
    {
        // 강화 모드 들어올 때
        RefreshSlotState(null);
    }

    public override void Hide()
    {
        // 강화 모드 나갈 때
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
    
    private void RefreshSlotState(ItemData item)
    {
        Debug.Log(item);
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = slot.CanUpgrade(item);
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
