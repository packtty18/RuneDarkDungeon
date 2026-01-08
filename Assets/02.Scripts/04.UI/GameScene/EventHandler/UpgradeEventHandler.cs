using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] TextMeshProUGUI _costTextUI;
    [SerializeField] TextMeshProUGUI _rateTextUI;
    
    private ISlotEventHandler _eventHandler;
    private List<UI_Slot> _slots;
    
    private UpgradeManager _upgradeManager;
    
    public void Initialize(UpgradeManager upgradeManager, List<UI_Slot>slots)
    {
        _eventHandler = new UnregisterEventHandler(upgradeManager);

        _slots = slots;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked += OnClickSlot;
        }
        
        _upgradeManager = upgradeManager;
        _upgradeManager.OnUpgradeDataChanged += SetUpgradeInfo;
    }
    
    private void OnDestroy()
    {
        if (_slots == null) return;
        foreach (var slot in _slots)
        {
            slot.OnSlotClicked -= OnClickSlot;
        }
        _upgradeManager.OnUpgradeDataChanged -= SetUpgradeInfo;
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
            slot.gameObject.SetActive(count-- > 0);
        }
    }
    
    private void OnClickSlot(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }
}
