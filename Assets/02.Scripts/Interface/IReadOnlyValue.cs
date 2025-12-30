using System;
using UnityEngine;

public interface IReadOnlyValue<T> where T : struct, IConvertible
{
   T Value { get; }

    void Subscribe(Action<T> action);
    void Unsubscribe(Action<T> action);
}
