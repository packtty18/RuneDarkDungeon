using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatBase<T> where T : struct, IConvertible
{
    private readonly SafeEvent<T> _onValueChanged = new();

    public void Subscribe(Action<T> action)
    {
        _onValueChanged.Subscribe(action);
    }

    public void Unsubscribe(Action<T> action)
    {
        _onValueChanged.Unsubscribe(action);
    }

    protected void Notify(T value)
    {
        _onValueChanged?.Invoke(value);
    }

    //더블형식으로 바꾼다
    protected double ToDouble(T value)
    {
        return Convert.ToDouble(value);
    }

    //T형식으로 바꾼다
    protected T FromDouble(double value)
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }

    //두개의 값이 같은지 체크한다.
    protected bool EqualsValue(T a, T b, double epsilon = 0.00001)
    {
        return Math.Abs(ToDouble(a) - ToDouble(b)) < epsilon;
    }

    //음수의 경우 0으로
    protected T ClampMinZero(T value)
    {
        return FromDouble(Math.Max(0, ToDouble(value)));
    }
}
