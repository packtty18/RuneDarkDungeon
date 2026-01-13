using UnityEngine;

public interface IForge : IReadOnlyForge
{
    void Register(ItemData item);
    void Unregister(ItemData item);
    void UnregisterAll();
    void Upgrade();
}
