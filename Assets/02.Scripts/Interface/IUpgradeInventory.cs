using UnityEngine;

public interface IUpgradeInventory : IInventory
{
    void Upgrade(ItemData item);
}
