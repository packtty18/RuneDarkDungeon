using System.Collections.Generic;
using System;

public interface IReadOnlyInventory
{
    IReadOnlyList<ItemData> Items { get; }
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
