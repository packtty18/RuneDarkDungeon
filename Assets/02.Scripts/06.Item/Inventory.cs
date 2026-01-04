using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Inventory : IReadOnlyInventory
{
    [SerializeField] private List<RuneData> _runes;
    public IReadOnlyList<RuneData> Runes => _runes;
    
    private SafeEvent<RuneData> _onRuneAdded = new();
    
    public void Add(RuneData rune)
    {
        _runes.Add(rune);
        Notify(rune);
    }
    
    public void Subscribe(Action<RuneData> action)
    {
        _onRuneAdded.Subscribe(action);
    }

    public void Unsubscribe(Action<RuneData> action)
    {
        _onRuneAdded.Unsubscribe(action);
    }

    private void Notify(RuneData value)
    {
        _onRuneAdded?.Invoke(value);
    }
}
