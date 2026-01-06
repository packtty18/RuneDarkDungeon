using UnityEngine;

public interface IInventory : IReadOnlyInventory
{
    void Add(IItem item);
    public void Remove(IItem item);
    public void Clear();
}
