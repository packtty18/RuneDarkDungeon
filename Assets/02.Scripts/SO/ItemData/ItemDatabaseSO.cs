using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<int, ItemSO> _itemDict;
    public int Count => _itemDict.Count;

    public ItemSO GetItemInfo(ItemData item)
    {
        return _itemDict.GetValueOrDefault(item.ID);
    }
}
