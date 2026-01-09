using UnityEngine;
using System.Collections.Generic;
using System;

public class UpgradeInventory : IUpgradeInventory
{
    private List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;

    private ItemUpgradeDataSO _upgradeDB;
    
    public ItemData TargetItem { get; private set; }
    public UpgradeData UpgradeInfo { get; private set; }

    private SafeEvent _onInventoryChanged = new();

    public UpgradeInventory(ItemUpgradeDataSO upgradeDB)
    {
        _upgradeDB = upgradeDB;
    }
    
    public bool TryAdd(ItemData item)
    {
        if (IsReadyToUpgrade) return false;
        if (TargetItem != null && !item.TypeEquals(TargetItem)) return false;

        _items.Add(item);

        if (_items.Count == 1)
        {
            SetTarget(item);
        }
        
        Notify();
        return true;
    }

    public void Remove(ItemData item)
    {
        if (_items.Remove(item))
        {
            if (_items.Count == 0) ResetTarget();
            Notify();
        }
    }

    public void Clear()
    {
        _items.Clear();
        ResetTarget();
        Notify();
    }

    private void SetTarget(ItemData item)
    {
        TargetItem = item;
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        UpgradeInfo = info ?? UpgradeData.Empty;
    }

    private void ResetTarget()
    {
        TargetItem = null;
        UpgradeInfo = UpgradeData.Empty;
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
    
    public bool IsReadyToUpgrade => TargetItem != null && IsFull;
    private bool IsFull => _items.Count == UpgradeInfo.Count;
    private bool IsEmpty => _items.Count == 0;
}

public interface IUpgradeInventory : IReadOnlyInventory
{
    ItemData TargetItem { get; }
    UpgradeData UpgradeInfo { get; }
    bool TryAdd(ItemData item);
    void Remove(ItemData item);
    void Clear();
    bool IsReadyToUpgrade { get; }
}
