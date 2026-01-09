using UnityEngine;

public class UpgradeManager
{
    private IngredientManager _ingredientManager;
    private IInventory _inventory;
    private ICurrency _goldData;

    public UpgradeManager(IngredientManager ingredientManager, IInventory inventory, ICurrency goldData)
    {
        _ingredientManager = ingredientManager;
        _inventory = inventory;
        _goldData = goldData;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (!_ingredientManager.CanRegister(item)) return false;
        
        _inventory.Remove(item);
        _ingredientManager.Register(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _ingredientManager.Unregister(item);
        _inventory.Add(item);
    }
    
    public void UnregisterAll()
    {
        foreach (var item in _ingredientManager.Ingredients.Items)
        {
            _inventory.Add(item);
        }
        _ingredientManager.Clear();
    }

    public void Upgrade()
    {
        if (!_ingredientManager.CanUpgrade() 
            || !_goldData.TryConsume(_ingredientManager.Cost)) return;
        
        var newItem = _ingredientManager.GetUpgradeResult();
        _inventory.Add(newItem);
        _ingredientManager.Clear();
    }
}
