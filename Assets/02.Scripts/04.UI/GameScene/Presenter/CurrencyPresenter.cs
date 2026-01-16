using UnityEngine;
using DG.Tweening;

public class CurrencyPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_GoldText _goldView;
    
    private IReadOnlyCurrency _gold;
    
    public void Initialize(IReadOnlyCurrency gold)
    {
        _gold = gold;
        _gold.Subscribe(RefreshGold);
        RefreshGold();
    }

    private void OnDestroy()
    {
        _gold.Unsubscribe(RefreshGold);
    }

    private void RefreshGold()
    {
        _goldView.Refresh(_gold.Amount);
    }
}