using UnityEngine;

public interface IDataHolder
{
    IInventory Inventory { get; }
    IEquipment Equipment { get; }
    ICurrency GoldData { get; }
    IForge Forge { get; }
    
    void Save();
}
