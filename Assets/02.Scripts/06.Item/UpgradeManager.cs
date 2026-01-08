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
        _targetType = null;
        _upgradeData = UpgradeData.Empty;
        OnUpgradeDataChanged?.Invoke(_upgradeData);
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
        if (!IsFull
            || !_goldData.TryConsume(_upgradeData.Cost)) return;
        
        ItemData newItem = new(_targetType.ID, _targetType.Grade + 1);
        _inventory.Add(newItem);
        _upgradeInventory.Clear();
    }
    
    private void RegisterTargetType(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _targetType = item;
        _upgradeData = info.Value;
        
        OnUpgradeDataChanged?.Invoke(_upgradeData);
    }

    private bool IsFull => _upgradeInventory.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Count == 0;
}
