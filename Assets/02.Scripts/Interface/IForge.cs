using UnityEngine;

public interface IForge : IReadOnlyForge
{
    bool TryRegister(ItemData item);
    void Unregister(ItemData item);
    void UnregisterAll();
    bool Upgrade(ICurrency currency, out ItemData item);
    void Notify();
}
