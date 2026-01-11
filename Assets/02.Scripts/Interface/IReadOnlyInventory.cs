using System.Collections.Generic;
using System;

public interface IReadOnlyInventory
{
    IReadOnlyList<ItemData> Items { get; }
    int Count { get; }
    bool CanAdd(ItemData item);
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
