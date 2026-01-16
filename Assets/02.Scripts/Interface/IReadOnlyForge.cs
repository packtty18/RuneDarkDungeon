using UnityEngine;

public interface IReadOnlyForge : IReadOnlyInventory
{
    UpgradeData UpgradeData { get; }
    bool CanRegister(ItemData item);
}
