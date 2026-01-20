using System.Collections.Generic;
using UnityEngine;

public class RewardPresenter : MonoBehaviour
{
    [Header("UI 및 데이터 연결")]
    [SerializeField] private UI_RewardPopup _rewardPopup;
    [SerializeField] private UI_SlotContainer _rewardUI;
    [SerializeField] private UI_GoldText _rewardGoldTextUI;

    private IReadOnlyList<ItemData> _items;
    private IReadOnlyCurrency _gold;

    public void Initialize(IReadOnlyList<ItemData> items, IReadOnlyCurrency gold)
    {
        _items = items;
        _gold = gold;
    }
    
    public void ShowReward()
    {
        _rewardUI.Refresh(_items);
        _rewardGoldTextUI.Refresh(_gold.Amount);
        
        _rewardPopup.PlayRewardAnimation();
    }
}
