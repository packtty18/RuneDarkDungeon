using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<int, ItemSO> _itemDict;

    public ItemSO GetItemInfo(int id)
    {
        return _itemDict.GetValueOrDefault(id);
    }
}