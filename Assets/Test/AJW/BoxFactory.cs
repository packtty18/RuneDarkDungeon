using UnityEngine;

public class BoxFactory : PoolFactory<Box>
{
    public BoxFactory(PoolManager poolManager) : base(poolManager, "Box")
    {
        if (!_poolManager.HasPool(_key))
        {
            throw new System.InvalidOperationException($"BoxFactory Start Error: Pool with key '{_key}' does not exist.");
        }
    }

    public Box Create()
    { 
        return CreateInternal();
    }

    public Box CreateAt(Vector3 position)
    {
        var box = CreateInternal();
        box.transform.position = position;
        return box;
    }
}
