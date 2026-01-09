using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;

    public ItemUpgradeDataSO UpgradeDB;
    public ItemDatabaseSO ItemDB;
    public ItemColorSO ColorDB;

    public ItemData ItemQ;
    public ItemData ItemE;
    public ItemData ItemR;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ItemDB.UseItem(gameObject, ItemQ);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ItemDB.UseItem(gameObject, ItemE);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemDB.UseItem(gameObject, ItemR);
        }
    }
}
