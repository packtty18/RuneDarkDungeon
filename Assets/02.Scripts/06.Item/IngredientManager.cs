using UnityEngine;

public class IngredientManager
{
    private readonly Inventory _ingredients = new();
    public IReadOnlyInventory Ingredients => _ingredients;

    private UpgradeDataSO _upgradeDB;
    
    private ItemData _targetType;
    private UpgradeData _upgradeData;
    
    public int Cost => _upgradeData.Cost;
    private int Count => _upgradeData.Count;
    public float Rate => _upgradeData.Rate;

    public IngredientManager(UpgradeDataSO upgradeDB)
    {
        _upgradeDB = upgradeDB;
    }
    
    public bool CanRegister(ItemData item)
    {
        if (_targetType == null) return true;
        return item.TypeEquals(_targetType) && _ingredients.Count < Count;
    }

    public void Register(ItemData item)
    {
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }
        _ingredients.Add(item);
    }

    public bool CanUpgrade()
    {
        if (_targetType == null || _ingredients.Count < Count) return false;
        return true;
    }
    
    public ItemData GetUpgradeResult()
    {
        ItemData newItem = new(_targetType.ID, _targetType.Grade + 1);
        return newItem;
    }

    public ItemData Unregister(ItemData item)
    {
        _ingredients.Remove(item);

        if (_ingredients.Count == 0)
        {
            _targetType = null;
        }
        
        return item;
    }

    public void Clear()
    {
        _ingredients.Clear();
        _targetType = null;
    }

    private void RegisterTargetType(ItemData item)
    {
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _targetType = item;
        _upgradeData = info.Value;
    }
}
