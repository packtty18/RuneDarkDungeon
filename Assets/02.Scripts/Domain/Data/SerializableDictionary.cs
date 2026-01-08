using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<SerializablePair<TKey, TValue>> _pairs;
    private Dictionary<TKey, TValue> _dict;
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _dict = new();
        foreach (var pair in _pairs)
        {
            if (_dict.TryAdd(pair.Key, pair.Value))
            {
                Debug.LogWarning($"중복된 Key 발견: {pair.Key}");
            }
        }
    }
    
    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }

    public TValue GetValueOrDefault(TKey key, TValue defaultValue = default)
    {
        return _dict.GetValueOrDefault(key, defaultValue);
    }
}
