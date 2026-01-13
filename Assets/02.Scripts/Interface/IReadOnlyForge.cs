using System;
using UnityEngine;

public interface IReadOnlyForge
{
    UpgradeData UpgradeData { get; }
    bool CanRegister(ItemData item);
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
