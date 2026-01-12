using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<int, ItemSO> _itemDict;
    [SerializeField] private ItemColorSO _colorDB;
    
    private ItemSO GetItemInfo(ItemData item)
    {
        return _itemDict.GetValueOrDefault(item.ID);
    }

    public SlotData GetSlotData(ItemData item)
    {
        ItemSO itemInfo = GetItemInfo(item);
        Color color = _colorDB.GetColor(item.Grade);

        return new SlotData(item, itemInfo, color);
    }

    public void UseItem(GameObject user, ItemData item, out float coolTime)
    {
        coolTime = 0;
        if (!_itemDict.TryGetValue(item.ID, out var info)) return;
        
        info.Use(user, item.Grade);
        coolTime = info.CoolTime;
    }
}
