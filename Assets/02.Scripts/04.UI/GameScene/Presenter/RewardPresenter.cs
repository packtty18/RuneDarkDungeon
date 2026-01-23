using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardPresenter : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UI_SequencePopup _sequencePopup;
    [SerializeField] private UI_SlotContainer _rewardUI;
    [SerializeField] private UI_GoldText _rewardGoldTextUI;
    [SerializeField] private TextMeshProUGUI _titleTextUI;
    
    [Header("설정")]
    [SerializeField] private string _victoryString = "VICTORY";
    [SerializeField] private string _defeatString = "DEFEAT";
    
    private IReadOnlyList<ItemData> _items;
    private IReadOnlyCurrency _gold;
    
    public void Initialize(IReadOnlyList<ItemData> items, IReadOnlyCurrency gold)
    {
        _items = items;
        _gold = gold;
    }

    public void ShowVictory()
    {
        SoundManager.Instance.Play(ESoundType.Stage_VictoryResult);
        _titleTextUI.SetText(_victoryString);
        ShowReward();
    }

    public void ShowDefeat()
    {
        SoundManager.Instance.Play(ESoundType.Stage_FailResult);
        _titleTextUI.SetText(_defeatString);
        ShowReward();
    }
    
    private void ShowReward()
    {
        _rewardUI.Refresh(_items);
        _rewardUI.Show();
        _sequencePopup.PlayAnimation();
        _rewardGoldTextUI.Refresh(_gold.Amount);
    }
}
