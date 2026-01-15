using UnityEngine;

public interface ICurrency : IReadOnlyCurrency
{
    void Add(int amount);
    bool TryConsume(int cost);
}
