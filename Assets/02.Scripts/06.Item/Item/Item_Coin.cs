using UnityEngine;

public class Item_Coin : ItemBase
{
    [Header("골드 획득량")]
    [SerializeField] private RangeData<int> _amountRange;
    
    protected override void OnCollected()
    {
        int amount = _amountRange.GetRandomValue();
        GameEvents.NotifyCoinCollected(amount);
    }
}
