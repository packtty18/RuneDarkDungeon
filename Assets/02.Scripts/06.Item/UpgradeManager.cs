using UnityEngine;

public class UpgradeManager
{
    private Ingredient _ingredient;
    private IInventory _inventory;
    private GoldData _goldData;

    public UpgradeManager(Ingredient ingredient, IInventory inventory, GoldData goldData)
    {
        _ingredient = ingredient;
        _inventory = inventory;
        _goldData = goldData;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (!_ingredient.CanRegister(item)) return false;
        
        _inventory.Remove(item);
        _ingredient.Register(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _ingredient.Unregister(item);
        _inventory.Add(item);
    }
    
    public void UnregisterAll()
    {
        foreach (var item in _ingredient.Ingredients.Items)
        {
            _inventory.Add(item);
        }
        _ingredient.Clear();
    }

    public void Upgrade()
    {
        if (!_ingredient.CanUpgrade() 
            || !_goldData.TryConsume(_ingredient.Cost)) return;
        
        var newItem = _ingredient.GetUpgradeResult();
        _inventory.Add(newItem);
        _ingredient.Clear();
    }
}
