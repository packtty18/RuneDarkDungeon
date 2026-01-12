using UnityEngine;
using TMPro;

public class UI_UpgradeInfo : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    
    public void Refresh(UpgradeData data)
    {
        _costTextUI.SetText("{0} 골드", data.Cost);
        _rateTextUI.SetText("{0}% 성공", data.Rate * 100);
    }
}
