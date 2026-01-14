using UnityEngine;

public interface IDataHandler
{
    IInventory Inventory { get; }
    IInventory UpgradeInventory { get; }
    IEquipment Equipment { get; }
    ICurrency GoldData { get; }
    IForge Forge { get; }
    
    void Save();
}
