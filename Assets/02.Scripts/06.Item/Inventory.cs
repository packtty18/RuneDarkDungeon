using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory : IInventory
{
    [SerializeField] private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private SafeEvent _onChanged = new();
    
    public void Add(ItemData item)
    {
        _items.Add(item);
        Notify();
    }

    public void Remove(ItemData item)
    {
        if (!_items.Remove(item)) return;
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
    
    public void Sort()
    {
        if (_items.Count <= 1) return;

        _items.Sort((itemA, itemB) => 
        {
            int idComp = itemA.ID.CompareTo(itemB.ID);
            if (idComp != 0) return idComp;

            return itemA.Grade.CompareTo(itemB.Grade);
        });

        Notify();
    }

    public void Clear()
    {
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
