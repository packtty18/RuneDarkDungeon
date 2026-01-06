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

    public void Upgrade()
    {
        if (!_ingredientManager.CanUpgrade()) return;
        
        var upgradeResult = _ingredientManager.GetUpgradeResult();
        GoldManager.Instance.UseGold(upgradeResult.cost);
        InventoryManager.Instance.AddItem(upgradeResult.newItem);
    }

    // Todo: 이벤트로 변경
    public ItemSO GetItemInfo(ItemData itemData)
    {
        return InventoryManager.Instance.GetItemInfo(itemData.ID);
    }
}
