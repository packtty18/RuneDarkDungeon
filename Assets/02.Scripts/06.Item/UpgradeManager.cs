using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private Inventory _upgradeInventory = new();
    private IInventory _inventory;
    private ICurrency _goldData;

    private UpgradeDataSO _upgradeDB;
    private ItemData _targetType;
    private UpgradeData _upgradeData;

    public IInventory UpgradeInventory => _upgradeInventory;
    
    public event Action<ItemData> OnTargetTypeChanged;
    public event Action<UpgradeData> OnUpgradeDataChanged; 
    
    public void Initialize(UpgradeDataSO upgradeDB, IInventory inventory, ICurrency goldData)
    {
        _upgradeDB = upgradeDB;
        _inventory = inventory;
        _goldData = goldData;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (item == null) return false;
        
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }

        if (!item.TypeEquals(_targetType) ||
            IsFull) return false;
        
        _inventory.Remove(item);
        _upgradeInventory.Add(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _upgradeInventory.Remove(item);
        _inventory.Add(item);

        if (!IsEmpty) return;
        ResetTargetType();
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
        
        ItemData newItem = new(_targetType.ID, _targetType.Grade.Next());
        _inventory.Add(newItem);
        _upgradeInventory.Clear();
        ResetTargetType();
    }
    
    private void RegisterTargetType(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _targetType = item;
        _upgradeData = info.Value;
        
        OnTargetTypeChanged?.Invoke(_targetType);
        OnUpgradeDataChanged?.Invoke(_upgradeData);
    }

    private void ResetTargetType()
    {
        _targetType = null;
        _upgradeData = UpgradeData.Empty;
        OnTargetTypeChanged?.Invoke(_targetType);
        OnUpgradeDataChanged?.Invoke(_upgradeData);
    }
    
    private bool IsFull => _upgradeInventory.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Count == 0;
}
