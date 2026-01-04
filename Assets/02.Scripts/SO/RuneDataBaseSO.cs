using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneDatabase", menuName = "Rune/RuneDatabase")]
public class RuneDatabaseSO : ScriptableObject
{
    [SerializeField] private List<RuneSO> _runes;
    private Dictionary<int, RuneSO> _runeDict;

    public void Initialize()
    {
        _runeDict = new();
        foreach (var rune in _runes)
        {
            if (_runeDict.TryAdd(rune.ID, rune)) continue;
            Debug.LogWarning($"중복된 ID 발견: {rune.ID}");
        }
    }

    public RuneSO GetRune(int id)
    {
        return _runeDict.GetValueOrDefault(id);
    }
}