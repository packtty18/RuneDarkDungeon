using System.Collections.Generic;
using System;
using UnityEngine;

public interface IReadOnlyInventory
{
    IReadOnlyList<ItemData> Items { get; }
    void Subscribe(Action<ItemData> action);
    void Unsubscribe(Action<ItemData> action);}
