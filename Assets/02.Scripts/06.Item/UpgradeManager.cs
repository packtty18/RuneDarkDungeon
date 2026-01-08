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
    
    public void Initialize(UpgradeDataSO upgradeDB, IInventory inventory, ICurrency goldData)
    {
        _upgradeDB = upgradeDB;
        _inventory = inventory;
        _goldData = goldData;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }

        if (!item.TypeEquals(_targetType) ||
            _isFull) return false;
        
        _inventory.Remove(item);
        _upgradeInventory.Add(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _upgradeInventory.Remove(item);
        _inventory.Add(item);
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
        if (!_isFull
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
    }

    private bool _isFull => _upgradeInventory.Count < _upgradeData.Count;
}
