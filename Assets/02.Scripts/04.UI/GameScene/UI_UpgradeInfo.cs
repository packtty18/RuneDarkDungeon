using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpgradeInfo : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
    public void Refresh(UpgradeData data, IReadOnlyList<UI_Slot> slots)
    {
        SetUpgradeInfo(data);
        SetUpgradeSlotCount(slots, data.Count);
    }

    private void SetUpgradeInfo(UpgradeData data)
    {
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
    }
    
    private void SetUpgradeSlotCount(IReadOnlyList<UI_Slot> slots, int count)
    {
        foreach (var slot in slots)
        {
            slot.SetActive(count-- > 0);
        }
    }
}
