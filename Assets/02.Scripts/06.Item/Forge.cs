using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Forge : IForge
{
    private readonly List<ItemData> _items = new();
    public IReadOnlyList<ItemData> Items => _items;
    
    private readonly ItemUpgradeDataSO _upgradeDB;
    private readonly ItemFactory _itemFactory;
    
    private ItemData _baseItem;
    private UpgradeData _upgradeData = UpgradeData.Empty;
    public UpgradeData UpgradeData => _upgradeData;

    private readonly SafeEvent _onChanged = new();

    public Forge(ItemFactory itemFactory, ItemUpgradeDataSO upgradeDB)
    {
        _itemFactory = itemFactory;
        _upgradeDB = upgradeDB;
    }

    public bool CanRegister(ItemData item)
    {
        if (IsFull || item == null || item.IsMaxGrade) return false;
        
        return _baseItem == null || _baseItem.TypeEquals(item);
    }
    
    public bool TryRegister(ItemData item)
    {
        if (!CanRegister(item)) return false;

        if (IsEmpty)
        {
            SetUpgradeData(item);
        }

        _items.Add(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _items.Remove(item);

        if (IsEmpty)
        {
            ResetUpgradeData();
        }
    }

    public void UnregisterAll()
    {
        _items.Clear();
        ResetUpgradeData(); 
    }

    public bool CanUpgrade(IReadOnlyCurrency currency)
    {
        return IsFull && currency.Amount >= _upgradeData.Cost;
    }
    
    public bool Upgrade(ICurrency currency, out ItemData item)
    {
        item = null;
        if (!IsFull || !currency.TryConsume(_upgradeData.Cost)) return false;

        if (Random.value < _upgradeData.Rate) return false;
        
        item = _itemFactory.CreateUpgradedItem(_baseItem);
        _items.Clear();

        ResetUpgradeData();
        return true;
    }

    private void SetUpgradeData(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;

        _baseItem = item;
        _upgradeData = info.Value;
    }

    private void ResetUpgradeData()
    {
        _baseItem = null;
        _upgradeData = UpgradeData.Empty;
    }

    public void Subscribe(Action action)
    {
        _onChanged.Subscribe(action);
    }

    public void Unsubscribe(Action action)
    {
        _onChanged.Unsubscribe(action);
    }
    
    public void Notify()
    {
        _onChanged?.Invoke();
    }

    private bool IsFull => _items.Count > 0 && _items.Count == _upgradeData.Count;
    private bool IsEmpty => _items.Count == 0;
}
