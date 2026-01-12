using UnityEngine;

public interface IDataHandler
{
    IInventory Inventory { get; }
    IInventory UpgradeInventory { get; }
    IEquipment Equipment { get; }
    ICurrency GoldData { get; }
    
    ItemDatabaseSO ItemDB { get; }
    ItemUpgradeDataSO UpgradeDB { get; }

    void Save();
}
