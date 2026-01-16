using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_UpgradeInfo : MonoBehaviour
{
    [SerializeField] private ItemFrameSO _frameDB;
    
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _costTextUI;
    [SerializeField] private TextMeshProUGUI _rateTextUI;
    [SerializeField] private Button _buttonImage;
    [SerializeField] private UI_Slot _resultSlot;
    
    public void Refresh(IReadOnlyForge forge, IReadOnlyCurrency currency)
    {
        var data = forge.UpgradeData;
        _costTextUI.SetText("{0}", data.Cost);
        _rateTextUI.SetText("성공 확률 {0}%", data.Rate * 100);
        _buttonImage.interactable = forge.CanUpgrade(currency);

        var item = forge.BaseItem;
        if (item == null)
        {
            _resultSlot.Clear();
            return;
        }
        
        var sprite = _frameDB.GetBorderSprite(item.Grade.Next());
        _resultSlot.SetItem(item, sprite);
    }
}
