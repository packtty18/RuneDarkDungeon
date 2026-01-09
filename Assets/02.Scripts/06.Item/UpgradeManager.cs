using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private IUpgradeInventory _upgradeInventory;
    private IInventory _inventory;
    private ICurrency _currency;
    
    public void Initialize(IUpgradeInventory upgradeInventory, IInventory inventory, ICurrency currency)
    {
        _upgradeInventory = upgradeInventory;
        _inventory = inventory;
        _currency = currency;
    }
    
    public void Register(ItemData item)
    {
        if (item == null) return;
        
        if (!_upgradeInventory.TryAdd(item)) return; 
        _inventory.Remove(item);
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
        if (!_upgradeInventory.IsReadyToUpgrade) return;
        
        int cost = _upgradeInventory.UpgradeInfo.Cost;
        if (!_currency.TryConsume(cost)) return;
        
        ItemData newItem = _upgradeInventory.TargetItem.GetUpgradedItem(); 
        _inventory.Add(newItem);
        
        _upgradeInventory.Clear();
    }
}
