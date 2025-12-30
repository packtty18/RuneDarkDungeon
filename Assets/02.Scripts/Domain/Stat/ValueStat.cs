using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ValueStat<T> : StatBase<T> , IReadOnlyValue<T>
    where T : struct, IConvertible
{
    [SerializeField] private T _value;
    public T Value => _value;
    public void Init(T value)
    {
        Set(value);
    }

    public void Increase(T amount)
    {
        Set(FromDouble(ToDouble(_value) + ToDouble(amount)));
    }

    public void Decrease(T amount)
    {
        Set(FromDouble(ToDouble(_value) - ToDouble(amount)));
    }

    public void Set(T newValue)
    {
        T clamped = ClampMinZero(newValue);

        if (EqualsValue(_value, clamped))
            return;

        _value = clamped;
        Notify(_value);
    }
}
