using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : IInventory
{
    [SerializeField] private List<IItem> _items = new();
    public IReadOnlyList<IItem> Items => _items;
    
    private SafeEvent<IItem> _onItemAdded = new();
    
    public int Count => _items.Count;
    
    public void Add(IItem item)
    {
        _items.Add(item);
        Notify(item);
    }

    public void Remove(IItem item)
    {
        _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }
    
    public void Subscribe(Action<IItem> action)
    {
        _onItemAdded.Subscribe(action);
    }

    public void Unsubscribe(Action<IItem> action)
    {
        _onItemAdded.Unsubscribe(action);
    }

    private void Notify(IItem item)
    {
        _onItemAdded?.Invoke(item);
    }
}
