using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpgradeFilter : UI_Base
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
    private IReadOnlyList<UI_Slot> _upgradeSlots;
    
    private UpgradeManager _upgradeManager;

    public void Initialize(UpgradeManager upgradeManager, IReadOnlyList<UI_Slot> upgradeSlots)
    {
        _upgradeSlots = upgradeSlots;
        _upgradeManager = upgradeManager;
        upgradeManager.Subscribe(Refresh);
    }

    private void OnDestroy()
    {
        _upgradeManager.Unsubscribe(Refresh);
    }

    private void Refresh()
    {
        SetUpgradeInfo();
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
