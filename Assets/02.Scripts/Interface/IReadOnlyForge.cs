using UnityEngine;

public interface IReadOnlyForge : IReadOnlyInventory
{
    ItemData BaseItem { get; }
    UpgradeData UpgradeData { get; }
    bool CanRegister(ItemData item);
    bool CanUpgrade(IReadOnlyCurrency currency);
}
