using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RewardPresenter : MonoBehaviour
{
    [Header("UI 및 데이터 연결")]
    [SerializeField] private UI_SequencePopup _sequencePopup;
    [SerializeField] private UI_SlotContainer _rewardUI;
    [SerializeField] private UI_GoldText _rewardGoldTextUI;
    
    [Space]
    [SerializeField] private float _startDelay = 1f;
    
    private IReadOnlyList<ItemData> _items;
    private IReadOnlyCurrency _gold;

    private Tween _delayedShow;
    
    public void Initialize(IReadOnlyList<ItemData> items, IReadOnlyCurrency gold)
    {
        _items = items;
        _gold = gold;
    }
    
    public void ShowReward()
    {
        _rewardUI.Refresh(_items);
        
        _delayedShow?.Kill();
        _delayedShow = DOVirtual.DelayedCall(_startDelay, () =>
        {
            _rewardUI.Show();
            _sequencePopup.PlayAnimation();
            _rewardGoldTextUI.Refresh(_gold.Amount);
        });
    }
}
