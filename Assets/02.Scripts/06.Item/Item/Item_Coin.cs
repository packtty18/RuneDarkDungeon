using UnityEngine;

public class Item_Coin : ItemBase
{
    [Header("골드 획득량")]
    [SerializeField] private RangeData<int> _amountRange;
    
    protected override void OnCollected()
    {
        int amount = Random.Range(_amountRange.Min, _amountRange.Max);
        GameEvents.NotifyCoinCollected(amount);
        Debug.Log("코인 획득");
    }
}
