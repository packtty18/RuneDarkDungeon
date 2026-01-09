using UnityEngine;

[System.Serializable]
public struct SerializablePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializablePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
