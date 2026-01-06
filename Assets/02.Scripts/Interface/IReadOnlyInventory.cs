using System.Collections.Generic;
using System;

public interface IReadOnlyInventory
{
    IReadOnlyList<IItem> Items { get; }
    void Subscribe(Action<IItem> action);
    void Unsubscribe(Action<IItem> action);
    int Count { get; }
}
