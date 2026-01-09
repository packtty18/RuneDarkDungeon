using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SlotController : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;

    private IUpgradeInventory _upgradeInventory;
    private IReadOnlyList<UI_Slot> _inventorySlots;
    private IReadOnlyList<UI_Slot> _upgradeSlots;
    
    private UpgradeManager _upgradeManager;

    public void Initialize(IUpgradeInventory upgradeInventory, IReadOnlyList<UI_Slot> inventorySlots, IReadOnlyList<UI_Slot> upgradeSlots)
    {
        _upgradeInventory = upgradeInventory;
        upgradeInventory.Subscribe(Refresh);
        
        _inventorySlots = inventorySlots;
        _upgradeSlots = upgradeSlots;
    }
    
    private void OnDestroy()
    {
        _upgradeInventory.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        RefreshSlotState(_upgradeInventory.TargetItem);
        SetUpgradeInfo(_upgradeInventory.UpgradeInfo);
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
