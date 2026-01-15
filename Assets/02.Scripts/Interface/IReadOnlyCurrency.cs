using System;
using UnityEngine;

public interface IReadOnlyCurrency
{
    int Amount { get; }
    void Subscribe(Action action);
    void Unsubscribe(Action action);
}
