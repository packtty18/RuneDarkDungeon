using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private IInventory _upgradeInventory;
    private IInventory _inventory;
    private ICurrency _goldData;

    private ItemUpgradeDataSO _upgradeDB;
    private ItemData _targetType;
    private UpgradeData _upgradeData;

    public ItemData TargetType => _targetType;
    public UpgradeData UpgradeData => _upgradeData;
    
    public void Initialize(ItemUpgradeDataSO upgradeDB, IInventory upgradeInventory, IInventory inventory, ICurrency goldData)
    {
        _upgradeDB = upgradeDB;
        _upgradeInventory = upgradeInventory;
        _inventory = inventory;
        _goldData = goldData;
    }
    
    public void Register(ItemData item)
    {
        if (item == null) return;
        
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }

        if (!item.TypeEquals(_targetType) ||
            IsFull) return;
        
        _inventory.Remove(item);
        _upgradeInventory.Add(item);
    }

    public void Unregister(ItemData item)
    {
        if (_upgradeInventory.Items.Count == 1)
        {
            ResetTargetType();
        }
        
        _inventory.Add(item);
        _upgradeInventory.Remove(item);
    }
    
    public void UnregisterAll()
    {
        foreach (var item in _upgradeInventory.Items)
        {
            _inventory.Add(item);
        }
        _upgradeInventory.Clear();
    }

    public void Upgrade()
    {
        if (_targetType == null 
            || !IsFull
            || !_goldData.TryConsume(_upgradeData.Cost)) return;
        
        ItemData newItem = _targetType.GetUpgradedItem();
        ResetTargetType();
        
        _inventory.Add(newItem);
        _upgradeInventory.Clear();
    }
    
    private void RegisterTargetType(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _targetType = item;
        _upgradeData = info.Value;
    }

    private void ResetTargetType()
    {
        _targetType = null;
        _upgradeData = UpgradeData.Empty;
    }
    
    private bool IsFull => _upgradeInventory.Items.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Items.Count == 0;
}
