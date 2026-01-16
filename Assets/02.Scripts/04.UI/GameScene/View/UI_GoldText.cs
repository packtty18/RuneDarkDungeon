using TMPro;
using DG.Tweening;
using UnityEngine;

public class UI_GoldText : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _goldTextUI;

    private Tween _tween;
    private float _displayGold;
    
    public void Refresh(int amount)
    {
        _tween?.Kill();
        
        _tween = DOTween.To(() => _displayGold, x => {
            _displayGold = x;
            _goldTextUI.text = $"{_displayGold:N0}";
        }, amount, 0.5f).SetEase(Ease.OutQuad);
    }
}
