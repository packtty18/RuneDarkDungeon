using UnityEngine;

public class InventoryManager : GlobalSingleton<InventoryManager>
{
    private Inventory _inventory;
    public IReadOnlyInventory Inventory => _inventory;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    
    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _itemDB.Initialize();
    }
    
    public void AddItem(ItemData item)
    {
        _inventory.Add(item);
    }
    
    public ItemSO GetItemInfo(int id)
    {
        return _itemDB.GetItem(id);
    }
}
