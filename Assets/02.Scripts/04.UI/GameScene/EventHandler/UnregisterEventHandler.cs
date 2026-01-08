using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnregisterEventHandler : ISlotEventHandler
{
    private UpgradeManager _upgradeManager;
    private TextMeshProUGUI _costTextUI;
    private TextMeshProUGUI _rateTextUI;
    
    private List<UI_Slot> _slots;
    private UI_Slot _resultSlot;
    
    public UnregisterEventHandler(UpgradeManager upgradeManager, List<UI_Slot> slots, TextMeshProUGUI costText, TextMeshProUGUI rateText)
    {
        _slots = slots;
        _costTextUI = costText;
        _rateTextUI = rateText;
        
        _upgradeManager = upgradeManager;
        _upgradeManager.OnUpgradeDataChanged += SetUpgradeInfo;
    }

    private void OnDestroy()
    {
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
        }
        _upgradeManager.OnUpgradeDataChanged -= SetUpgradeInfo;
    }
    
    public void OnClickSlot(UI_Slot slot)
    {
        if (slot.IsEmpty) return;
        _upgradeManager.Unregister(slot.Data.Item);
    }
    
    private void SetUpgradeInfo(UpgradeData data)
    {
        SetSlotCount(data.Count);
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
    }
    
    private void SetSlotCount(int count)
    {
        foreach (var slot in _slots)
        {
            slot.gameObject.SetActive(count-- > 0 ? true : false);
        }
    }

    public void OnHoverSlot(UI_Slot slot) { }
    public void OnEnter() { }
    public void OnExit() { }
}
