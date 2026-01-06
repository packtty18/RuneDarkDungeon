using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Item/ItemDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private List<ItemSO> _items;
    private Dictionary<int, ItemSO> _itemDict;

    private void OnEnable()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        _itemDict = new();
        foreach (var item in _items)
        {
            if (_itemDict.TryAdd(item.ID, item)) continue;
            Debug.LogWarning($"중복된 ID 발견: {item.ID}");
        }
    }

    public ItemSO GetItemInfo(int id)
    {
        return _itemDict.GetValueOrDefault(id);
    }
}