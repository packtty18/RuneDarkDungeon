using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RewardPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SequencePopup _sequencePopup;
    [SerializeField] private UI_SlotContainer _rewardUI;
    [SerializeField] private UI_GoldText _rewardGoldTextUI;
    [SerializeField] private TextMeshProUGUI _titleTextUI;
    
    [Header("설정")]
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private string _victoryString = "VICTORY";
    [SerializeField] private string _defeatString = "DEFEAT";
    
    private IReadOnlyList<ItemData> _items;
    private IReadOnlyCurrency _gold;

    private Tween _delayedShow;
    
    public void Initialize(IReadOnlyList<ItemData> items, IReadOnlyCurrency gold)
    {
        _items = items;
        _gold = gold;
    }

    public void ShowVictory()
    {
        _titleTextUI.SetText(_victoryString);
        ShowReward();
    }

    public void ShowDefeat()
    {
        _titleTextUI.SetText(_defeatString);
        ShowReward();
    }
    
    private void ShowReward()
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
