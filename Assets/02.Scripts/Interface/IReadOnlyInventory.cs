using System.Collections.Generic;
using System;
using UnityEngine;

public interface IReadOnlyInventory
{
    IReadOnlyList<RuneData> Runes { get; }
    void Subscribe(Action<RuneData> action);
    void Unsubscribe(Action<RuneData> action);}
