using UnityEngine;

public interface IDataHolder
{
    IInventory Inventory { get; }
    IInventory UpgradeInventory { get; }
    IEquipment Equipment { get; }
    ICurrency GoldData { get; }
    IForge Forge { get; }
    
    void Save();
}
