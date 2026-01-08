using System.Collections.Generic;
using System;

public interface IReadOnlyInventory
{
    IReadOnlyList<ItemData> Items { get; }
    void Subscribe(Action<ItemData> addAction, Action<ItemData> removeAction);
    void Unsubscribe(Action<ItemData> addAction, Action<ItemData> removeAction);
}
