using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SlotController : MonoBehaviour
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
        _upgradeManager.OnUpgradeDataChanged += SetUpgradeInfo;
        _upgradeManager.OnTargetTypeChanged += RefreshSlotState;
        
        _inventorySlots = inventorySlots;
        _upgradeSlots = upgradeSlots;
    }
    
    private void OnDestroy()
    {
        _upgradeManager.OnUpgradeDataChanged -= SetUpgradeInfo;
        _upgradeManager.OnTargetTypeChanged -= RefreshSlotState;
    }
    
    public void ResetSlotState()
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            slot.SetInteractable(true);
        }
    }
    
    public void RefreshSlotState()
    {
        RefreshSlotState(null);
    }
    
    private void RefreshSlotState(ItemData targetItem)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            bool isOn = slot.CanUpgrade(targetItem);
            slot.SetInteractable(isOn);
        }
    }
    
    private void SetUpgradeInfo(UpgradeData data)
    {
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
