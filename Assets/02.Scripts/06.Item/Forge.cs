using System;
using UnityEngine;

public class Forge : IForge
{
    private IInventory _upgradeInventory;
    private IInventory _inventory;
    private ICurrency _currency;

    private ItemUpgradeDataSO _upgradeDB;
    
    private ItemData _baseItem;
    private UpgradeData _upgradeData = UpgradeData.Empty;
    public UpgradeData UpgradeData => _upgradeData;

    private SafeEvent _onChanged = new();

    public Forge(ItemUpgradeDataSO upgradeDB, IInventory upgradeInventory, IInventory inventory, ICurrency currency)
    {
        _upgradeDB = upgradeDB;
        _upgradeInventory = upgradeInventory;
        _inventory = inventory;
        _currency = currency;
    }

    public bool CanRegister(ItemData item)
    {
        if (IsFull || item == null || item.IsMaxGrade) return false;
        
        return _baseItem == null || _baseItem.TypeEquals(item);
    }
    
    public void Register(ItemData item)
    {
        if (!CanRegister(item)) return;

        if (IsEmpty)
        {
            SetUpgradeData(item);
        }

        _inventory.Remove(item);
        _upgradeInventory.Add(item);

        Notify();
    }

    public void Unregister(ItemData item)
    {
        _upgradeInventory.Remove(item);
        _inventory.Add(item);

        if (IsEmpty)
        {
            ResetUpgradeData();
        }

        Notify();
    }

    public void UnregisterAll()
    {
        foreach (var item in _upgradeInventory.Items)
        {
            _inventory.Add(item);
        }

        _upgradeInventory.Clear();
        ResetUpgradeData(); 
    }

    public void Upgrade()
    {
        if (!IsFull || !_currency.TryConsume(_upgradeData.Cost)) return;

        ItemData newItem = _baseItem.GetUpgradedItem();
        _inventory.Add(newItem);
        _upgradeInventory.Clear();

        ResetUpgradeData();
        Notify();
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
    
    private void Notify()
    {
        _onChanged?.Invoke();
    }

    private bool IsFull => _upgradeInventory.Count > 0 && _upgradeInventory.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Count == 0;
}
