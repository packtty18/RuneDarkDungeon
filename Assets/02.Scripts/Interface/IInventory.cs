using UnityEngine;

public interface IInventory : IReadOnlyInventory
{
    void Add(ItemData item);
    void Remove(ItemData item);
    void Clear();
}
