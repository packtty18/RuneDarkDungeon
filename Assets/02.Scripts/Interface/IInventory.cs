using UnityEngine;

public interface IInventory : IReadOnlyInventory
{
    void Add(ItemData item);
    void Remove(ItemData item);
    void Swap(int indexA, int indexB);
    void Clear();
}
