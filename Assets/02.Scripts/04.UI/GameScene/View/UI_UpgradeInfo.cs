using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradeInfo : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _availableSprite;
    [SerializeField] private Sprite _unavailableSprite;
    
    public void Refresh(IReadOnlyForge forge, IReadOnlyCurrency currency)
    {
        var data = forge.UpgradeData;
        _costTextUI.SetText("{0}", data.Cost);
        _rateTextUI.SetText("성공 확률 {0}%", data.Rate * 100);

        if (forge.CanUpgrade(currency))
        {
            _buttonImage.sprite = _availableSprite;
        }
        else
        {
            _buttonImage.sprite = _unavailableSprite;
        }
    }
}
