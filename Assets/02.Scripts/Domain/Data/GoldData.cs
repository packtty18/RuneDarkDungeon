using System;
using UnityEngine;

[Serializable]
public class GoldData : ICurrency
{
    [SerializeField] private int _amount;
    public int Amount => _amount;
    
    private readonly SafeEvent _onChanged = new();

    public GoldData(int amount = 0)
    {
        _amount = amount;
    }
    
    private void SetAmount(int amount)
    {
        int clamped = Math.Max(0, amount);

        if (_amount == clamped) return;

        _amount = clamped;
        Notify();
    }
    
    public void Add(int amount)
    {
        SetAmount(_amount + amount);
    }

    public bool TryConsume(int cost)
    {
        if (_amount < cost) return false;
        
        SetAmount(_amount - cost);
        return true;
    }

    public void Subscribe(Action action)
    {
        _onChanged.Subscribe(action);
    }

    public void Unsubscribe(Action action)
    {
        _onChanged.Unsubscribe(action);
    }
    
    private void Notify()
    {
        _onChanged?.Invoke();
    }
}
