using System;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private IInventory _upgradeInventory;
    private IInventory _inventory;
    private ICurrency _currency;

    private ItemUpgradeDataSO _upgradeDB;
    private UpgradeData _upgradeData;
    
    public UpgradeData UpgradeData => _upgradeData;

    public SafeEvent OnChanged = new();
    
    public void Initialize(ItemUpgradeDataSO upgradeDB, IInventory upgradeInventory, IInventory inventory, ICurrency currency)
    {
        _upgradeDB = upgradeDB;
        _upgradeInventory = upgradeInventory;
        _inventory = inventory;
        _currency = currency;
    }
    
    public void Register(ItemData item)
    {
        if (IsFull || !_upgradeInventory.CanAdd(item)) return;

        if (IsEmpty)
        {
            SetUpgradeData(item);
        }
        
        _inventory.Remove(item);
        _upgradeInventory.Add(item);
        
        OnChanged?.Invoke();
    }

    public void Unregister(ItemData item)
    {
        _upgradeInventory.Remove(item);
        _inventory.Add(item);
        
        if (IsEmpty)
        {
            ResetUpgradeData();
        }
        
        OnChanged?.Invoke();
    }
    
    public void UnregisterAll()
    {
        foreach (var item in _upgradeInventory.Items)
        {
            _inventory.Add(item);
        }
        _upgradeInventory.Clear();
        ResetUpgradeData();
        
        OnChanged?.Invoke();
    }

    public void Upgrade()
    {
        if (!IsFull || !_currency.TryConsume(_upgradeData.Cost)) return;
        
        ItemData newItem = _upgradeInventory.Items.First().GetUpgradedItem();
        ResetUpgradeData();
        
        _inventory.Add(newItem);
        _upgradeInventory.Clear();
        
        OnChanged?.Invoke();
    }
    
    private void SetUpgradeData(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;

        _upgradeData = info.Value;
    }

    private void ResetUpgradeData()
    {
        _upgradeData = UpgradeData.Empty;
    }
    
    private bool IsFull => _upgradeInventory.Count > 0 && _upgradeInventory.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Count == 0;
}
