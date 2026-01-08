using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : IInventory
{
    [SerializeField] private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent<ItemData> _onItemAdded = new();
    private SafeEvent<ItemData> _onItemRemoved = new();
    
    public int Count => _items.Count;
    
    public void Add(ItemData item)
    {
        _items.Add(item);
        NotifyAdded(item);
    }

    public void Remove(ItemData item)
    {
        _items.Remove(item);
        NotifyRemoved(item);
    }

    public void Clear()
    {
        _items.Clear();
    }
    
    public void Subscribe(Action<ItemData> addAction, Action<ItemData> removeAction)
    {
        _onItemAdded.Subscribe(addAction);
        _onItemRemoved.Subscribe(removeAction);
    }

    public void Unsubscribe(Action<ItemData> addAction, Action<ItemData> removeAction)
    {
        _onItemAdded.Unsubscribe(addAction);
        _onItemRemoved.Unsubscribe(removeAction);
    }

    private void NotifyAdded(ItemData item)
    {
        _onItemAdded?.Invoke(item);
    }
    
    private void NotifyRemoved(ItemData item)
    {
        _onItemRemoved?.Invoke(item);
    }
}
