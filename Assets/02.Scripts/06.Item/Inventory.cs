using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : IInventory
{
    [SerializeField] private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent<ItemData> _onItemAdded = new();
    
    public int Count => _items.Count;
    
    public void Add(ItemData item)
    {
        _items.Add(item);
        Notify(item);
    }

    public void Remove(ItemData item)
    {
        _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }
    
    public void Subscribe(Action<ItemData> action)
    {
        _onItemAdded.Subscribe(action);
    }

    public void Unsubscribe(Action<ItemData> action)
    {
        _onItemAdded.Unsubscribe(action);
    }

    private void Notify(ItemData item)
    {
        _onItemAdded?.Invoke(item);
    }
}
