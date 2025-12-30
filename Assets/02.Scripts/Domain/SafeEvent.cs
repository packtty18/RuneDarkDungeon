using System;
using System.Collections.Generic;

[Serializable]
public class SafeEvent
{
     private event Action _event;
     [NonSerialized]private HashSet<Action> _listeners;

    public SafeEvent()
    {
        _listeners = new HashSet<Action>();
    }

    public void Subscribe(Action listener)
    {
        if (_listeners == null) _listeners = new HashSet<Action>();
        if (_listeners.Add(listener))
            _event += listener;
    }

    public void Unsubscribe(Action listener)
    {
        if (_listeners == null) return;
        if (_listeners.Remove(listener))
            _event -= listener;
    }

    public void Invoke()
    {
        _event?.Invoke();
    }
}

[Serializable]
public class SafeEvent<T>
{
     private event Action<T> _event;
    [NonSerialized] private HashSet<Action<T>> _listeners;

    public SafeEvent()
    {
        _listeners = new HashSet<Action<T>>();
    }

    public void Subscribe(Action<T> listener)
    {
        if (_listeners == null) _listeners = new HashSet<Action<T>>();
        if (_listeners.Add(listener))
            _event += listener;
    }

    public void Unsubscribe(Action<T> listener)
    {
        if (_listeners == null) return;
        if (_listeners.Remove(listener))
            _event -= listener;
    }

    public void Invoke(T value)
    {
        _event?.Invoke(value);
    }
}
