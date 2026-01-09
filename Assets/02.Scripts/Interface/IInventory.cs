using UnityEngine;

public interface IInventory : IReadOnlyInventory
{
    void Add(ItemData item);
    void Remove(ItemData item);
    void Swap(ItemData itemA, ItemData itemB);
    void Clear();
}
