using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeEventHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
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
        _upgradeManager.OnUpgradeDataChanged += SetUpgradeInfo;
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
        _upgradeManager.OnUpgradeDataChanged -= SetUpgradeInfo;
    }
    
    private void UpgradeMode(bool isOn)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty) continue;
            slot.SetUpgradeMode(isOn);
        }
    }
    
    private void SetSlotCount(int count)
    {
        foreach (var slot in _upgradeSlots)
        {
            if (count-- > 0)
            {
                slot.gameObject.SetActive(true);
            }
            else
            {
                slot.gameObject.SetActive(false);
            }
        }
    }

    private void SetUpgradeInfo(UpgradeData data)
    {
        SetSlotCount(data.Count);
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
    }

    private void OnClickInventorySlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.TryRegister(slot.Data.Item);
    }

    private void OnClickUpgradeSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Unregister(slot.Data.Item);
    }
}
