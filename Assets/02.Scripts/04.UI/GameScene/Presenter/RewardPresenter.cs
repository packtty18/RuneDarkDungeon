using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardPresenter : MonoBehaviour
{
    [Header("UI 및 데이터 연결")]
    [SerializeField] private UI_SlotContainer _rewardUI;
    [SerializeField] private TextMeshProUGUI _rewardGoldTextUI;

    private IReadOnlyList<ItemData> _items;
    private IReadOnlyCurrency _gold;

    public void Initialize(IReadOnlyList<ItemData> items, IReadOnlyCurrency gold)
    {
        _items = items;
        _gold = gold;
    }
    
    public void ShowReward()
    {
        _rewardUI.Show();
        
        _rewardGoldTextUI.SetText("+ {0}", _gold.Amount);
        _rewardUI.Refresh(_items);
    }

    public void HideReward()
    {
        _rewardUI.Hide();
    }
}
