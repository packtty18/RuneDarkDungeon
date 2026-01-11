using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInventory : IInventory
{
    private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent _onChanged = new();

    private ItemData _targetType;
    
    public int Count => _items.Count;

    public bool CanAdd(ItemData item)
    {
        if (item == null || item.IsMaxGrade) return false;
        
        return _targetType == null || _targetType.TypeEquals(item);
    }
    
    public void Add(ItemData item)
    {
        if (!CanAdd(item)) return;

        if (_items.Count == 0)
        {
            _targetType = item;
        }
        _items.Add(item);
        Notify();
    }

    public void Remove(ItemData item)
    {
        if (!_items.Remove(item)) return;
        
        if (_items.Count == 0)
        {
            _targetType = null;
        }
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
        _targetType = null;
        _items.Clear();
        Notify();
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
