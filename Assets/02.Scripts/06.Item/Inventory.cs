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
    
    private void EnsureCapacity(int index)
    {
        while (_items.Count <= index)
        {
            _items.Add(null);
        }
    }
    
    public void Add(ItemData item)
    {
        int emptyIndex = _items.IndexOf(null);
        if (emptyIndex < 0)
        {
            _items.Add(item);
        }
        else
        {
            _items[emptyIndex] = item;
        }
        Notify();
    }

    public void Remove(ItemData item)
    {
        int index = _items.IndexOf(item);
        if (index < 0) return;
        
        _items[index] = null;
        Notify();
    }

    public void Swap(int indexA, int indexB)
    {
        if (indexA == indexB) return;
        EnsureCapacity(Mathf.Max(indexA, indexB));
        
        (_items[indexA], _items[indexB]) = (_items[indexB], _items[indexA]);
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
