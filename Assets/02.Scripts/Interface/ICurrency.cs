using UnityEngine;

public interface ICurrency : IReadOnlyValue<int>
{
    void Add(int amount);
    bool TryConsume(int cost);
}
