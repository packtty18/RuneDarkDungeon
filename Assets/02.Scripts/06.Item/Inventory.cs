using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : IInventory
{
    [SerializeField] private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent _onInventoryChanged = new();
    
    public int Count => _items.Count;
    
    public void Add(ItemData item)
    {
        _items.Add(item);
        Notify();
    }

    public void Remove(ItemData item)
    {
        _items.Remove(item);
        Notify();
    }

    public void Swap(ItemData itemA, ItemData itemB)
    {
        if (itemA == null || itemB == null || itemA == itemB) return;

        int indexA = _items.IndexOf(itemA);
        int indexB = _items.IndexOf(itemB);

        _items[indexA] = itemB;
        
        _items[indexB] = itemA;

        Notify();
    }

    public void Clear()
    {
        _items.Clear();
        Notify();
    }
    
    public void Subscribe(Action action)
    {
        _onInventoryChanged.Subscribe(action);
    }

    public void Unsubscribe(Action action)
    {
        _onInventoryChanged.Unsubscribe(action);
    }

    private void Notify()
    {
        _onInventoryChanged?.Invoke();
    }
}
