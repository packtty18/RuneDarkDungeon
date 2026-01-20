using UnityEngine;
using Sirenix.OdinInspector;

public class RewardTest : MonoBehaviour
{
    [SerializeField] private RewardManager _rewardManager;
    [SerializeField] private RewardPresenter _presenter;

    private void Start()
    {
        _rewardManager.Initialize(RuneUser.Instance);
    }
    
    [Title("Test Settings")]
    [SerializeField] private int _testGoldAmount = 1000;
    [SerializeField] private EItemGrade _testItemGrade = EItemGrade.Rare;

    [Button("1. Add Test Rewards (Gold & Rune)"), GUIColor(0.5f, 1f, 0.5f)]
    public void Test_AddRewards()
    {
        GameEvents.NotifyCoinCollected(_testGoldAmount);
        GameEvents.NotifyRuneCollected(_testItemGrade);
    }

    [Button("2. Simulate Victory (Show UI)"), GUIColor(0.5f, 0.5f, 1f)]
    public void Test_Victory()
    {
        Debug.Log(_rewardManager.RewardGold.Amount);
        Debug.Log(string.Join(", ", _rewardManager.RewardItems));
        _presenter.ShowReward();
    }
}
