using UnityEngine;

public class IngredientManager
{
    private readonly Inventory _ingredients = new();
    public IReadOnlyInventory Ingredients => _ingredients;

    private UpgradeDataSO _upgradeDB;
    
    private ItemData _targetType;
    private int _price;
    private int _count;

    public IngredientManager(UpgradeDataSO upgradeDB)
    {
        _upgradeDB = upgradeDB;
    }
    
    public bool CanRegister(ItemData item)
    {
        if (_targetType == null) return true;
        return item.TypeEquals(_targetType) && _ingredients.Count < _count;
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
        if (_targetType == null || _ingredients.Count < _count) return false;
        return true;
    }
    
    public (ItemData newItem, int cost) GetUpgradeResult()
    {

        ItemData newItem = new(_targetType.ID, _targetType.Grade + 1);
        return (newItem, _price);
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

    private void RegisterTargetType(ItemData item)
    {
        _targetType = item;
        
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _price = info.Value.UpgradePrice;
        _count = info.Value.IngredientCount;
    }
}
