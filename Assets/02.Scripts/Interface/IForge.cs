using System;
using UnityEngine;

public interface IForge
{
    UpgradeData UpgradeData { get; }
    void Register(ItemData item);
    void Unregister(ItemData item);
    void UnregisterAll();
    void Upgrade();
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
