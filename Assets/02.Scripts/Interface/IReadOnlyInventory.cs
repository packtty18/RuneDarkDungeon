using System.Collections.Generic;
using System;

public interface IReadOnlyInventory
{
    IReadOnlyList<ItemData> Items { get; }
    void Subscribe(Action<ItemData> action);
    void Unsubscribe(Action<ItemData> action);
}
