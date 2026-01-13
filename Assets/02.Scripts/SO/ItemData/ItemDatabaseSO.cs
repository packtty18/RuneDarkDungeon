using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<int, ItemSO> _itemDict;
    [SerializeField] private ItemColorSO _colorDB;
    
    public ItemSO GetItemInfo(ItemData item)
    {
        return _itemDict.GetValueOrDefault(item.ID);
    }

    public SlotData GetSlotData(ItemData item)
    {
        ItemSO itemInfo = GetItemInfo(item);
        Color color = _colorDB.GetColor(item.Grade);

        return new SlotData(item, itemInfo, color);
    }
}
