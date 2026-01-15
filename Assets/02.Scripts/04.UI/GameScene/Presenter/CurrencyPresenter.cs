using TMPro;
using UnityEngine;

public class CurrencyPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private TextMeshProUGUI _goldTextUI;
    
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
        _goldTextUI.SetText("{0}", _gold.Amount);
    }
}
