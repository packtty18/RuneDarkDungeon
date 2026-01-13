using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializablePair<TKey, TValue>> _pairs = new();

    public void OnBeforeSerialize()
    {
        _pairs.Clear();
        foreach (var pair in this)
        {
            _pairs.Add(new SerializablePair<TKey, TValue>(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        foreach (var pair in _pairs)
        {
            if (TryAdd(pair.Key, pair.Value)) continue; 
            Debug.LogWarning($"중복된 Key 발견: {pair.Key}");
        }
    }

    public TValue GetValueOrDefault(TKey key, TValue defaultValue = default)
    {
        return TryGetValue(key, out var value) ? value : defaultValue;
    }
}
