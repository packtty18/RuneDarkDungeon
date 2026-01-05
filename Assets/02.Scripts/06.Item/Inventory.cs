using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Inventory : IReadOnlyInventory
{
    [SerializeField] private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent<ItemData> _onItemAdded = new();
    
    public void Add(ItemData item)
    {
        _items.Add(item);
        Notify(item);
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
