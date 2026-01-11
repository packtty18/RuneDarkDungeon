using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private IInventory _upgradeInventory;
    private IInventory _inventory;
    private ICurrency _currency;

    private ItemUpgradeDataSO _upgradeDB;
    private ItemData _baseItem;
    private UpgradeData _upgradeData = UpgradeData.Empty;

    public UpgradeData UpgradeData => _upgradeData;

    private SafeEvent _onChanged = new();

    public void Initialize(ItemUpgradeDataSO upgradeDB, IInventory upgradeInventory, IInventory inventory,
        ICurrency currency)
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

        _onChanged?.Invoke();
    }

    public void Unregister(ItemData item)
    {
        _upgradeInventory.Remove(item);
        _inventory.Add(item);

        if (IsEmpty)
        {
            ResetUpgradeData();
        }

        _onChanged?.Invoke();
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
        ResetUpgradeData();

        _inventory.Add(newItem);
        _upgradeInventory.Clear();

        _onChanged?.Invoke();
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

private bool IsFull => _upgradeInventory.Count > 0 && _upgradeInventory.Count == _upgradeData.Count;
    private bool IsEmpty => _upgradeInventory.Count == 0;
}
