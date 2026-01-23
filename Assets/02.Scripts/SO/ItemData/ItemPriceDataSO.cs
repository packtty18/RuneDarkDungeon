using UnityEngine;

[CreateAssetMenu(fileName = "ItemPriceData", menuName = "Item/PriceData")]
public class ItemPriceDataSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, int> _priceDict;
    
    public int GetPrice(ItemData item)
    {
        return _priceDict.GetValueOrDefault(item.Grade);
    }
}
