using UnityEngine;

public interface IDataHandler
{
    IInventory Inventory { get; }
    IUpgradeInventory UpgradeInventory { get; }
    ICurrency GoldData { get; }
    
    ItemDatabaseSO ItemDB { get; }
    ItemUpgradeDataSO UpgradeDB { get; }

    void Save();
}
