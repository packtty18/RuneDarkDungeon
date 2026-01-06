using UnityEngine;

public interface IInventory : IReadOnlyInventory
{
    void Add(ItemData item);
    public void Remove(ItemData item);
    public void Clear();
}
