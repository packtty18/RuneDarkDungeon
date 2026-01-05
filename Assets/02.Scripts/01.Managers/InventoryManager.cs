using UnityEngine;

public class InventoryManager : GlobalSingleton<InventoryManager>
{
    private Inventory _inventory;
    public IReadOnlyInventory Inventory => _inventory;
    
    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
    }
    
    public void AddItem(ItemData item)
    {
        _inventory.Add(item);
    }
}
