using UnityEngine;

public class UpgradeManager
{
    private IngredientManager _ingredientManager;

    public UpgradeManager(IngredientManager ingredientManager)
    {
        _ingredientManager = ingredientManager;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (!_ingredientManager.CanRegister(item)) return false;
        
        InventoryManager.Instance.RemoveItem(item);
        _ingredientManager.Register(item);
        return true;
    }

    public void Unregister(ItemData item)
    {
        _ingredientManager.Unregister(item);
        InventoryManager.Instance.AddItem(item);
    }
    
    public void UnregisterAll()
    {
        foreach (var item in _ingredientManager.Ingredients.Items)
        {
            InventoryManager.Instance.AddItem(item);
        }
        _ingredientManager.Clear();
    }

    public void Upgrade()
    {
        if (!_ingredientManager.CanUpgrade() 
            || !GoldManager.Instance.UseGold(_ingredientManager.Cost)) return;
        
        var newItem = _ingredientManager.GetUpgradeResult();
        InventoryManager.Instance.AddItem(newItem);
        _ingredientManager.Clear();
    }

    // Todo: 이벤트로 변경
    public ItemSO GetItemInfo(ItemData itemData)
    {
        return InventoryManager.Instance.GetItemInfo(itemData.ID);
    }
}
