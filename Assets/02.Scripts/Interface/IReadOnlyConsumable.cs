using System;


public interface IReadOnlyConsumable<T> where T : struct, IConvertible
{
    T Max { get; }
    T Current { get; }
    float Regen { get; }

    bool IsFull();
    bool IsEmpty();
    float GetRatio();
    void Consume(T amount);
    void Regenerate(float deltaTime);


    void Subscribe(Action<T> action);
    void Unsubscribe(Action<T> action);
}
