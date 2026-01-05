using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneDatabase", menuName = "Item/RuneDatabase")]
public class ItemDatabaseSO : ScriptableObject
{
    [SerializeField] private List<ItemSO> _runes;
    private Dictionary<int, ItemSO> _runeDict;

    public void Initialize()
    {
        _runeDict = new();
        foreach (var rune in _runes)
        {
            if (_runeDict.TryAdd(rune.ID, rune)) continue;
            Debug.LogWarning($"중복된 ID 발견: {rune.ID}");
        }
    }

    public ItemSO GetItem(int id)
    {
        return _runeDict.GetValueOrDefault(id);
    }
}