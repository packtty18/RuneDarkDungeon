using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeEventHandler : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
    private IReadOnlyList<UI_Slot> _slots;
    private ISlotEventHandler _eventHandler;
    
    private UpgradeManager _upgradeManager;
    
    public void Initialize(UpgradeManager upgradeManager, IReadOnlyList<UI_Slot>slots)
    {
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
    
    public void SetMode(ISlotEventHandler eventHandler)
    {
        _eventHandler?.OnExit();
        _eventHandler = eventHandler;
        _eventHandler.OnEnter();
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
            slot.SetActive(count-- > 0);
        }
    }
    
    private void OnClickSlot(UI_Slot slot)
    {
        _eventHandler.OnClickSlot(slot);
    }
}
