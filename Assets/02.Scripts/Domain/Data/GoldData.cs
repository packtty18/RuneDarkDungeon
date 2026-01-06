using System;
using UnityEngine;

[Serializable]
public class GoldData : StatBase<int>, IReadOnlyValue<int>
{
    [SerializeField] private int _value;
    public int Value => _value;

    private void SetAmount(int amount)
    {
        int clamped = Math.Max(0, amount);

        if (_value == clamped) return;

        _value = clamped;
        Notify(_value);
    }
    
    public void Add(int amount)
    {
        SetAmount(_value + amount);
    }

    public bool TryConsume(int cost)
    {
        if (_value < cost) return false;
        
        SetAmount(_value - cost);
        return true;
    }
}