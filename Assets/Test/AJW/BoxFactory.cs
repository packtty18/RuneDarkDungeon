using UnityEngine;

public class BoxFactory : PoolFactory<Box>
{
    public BoxFactory(PoolManager poolManager) : base(poolManager, EPoolType.Box)
    {
        if (!_poolManager.HasPool(_type))
        {
            throw new System.InvalidOperationException($"BoxFactory Start Error: Pool with key '{_type}' does not exist.");
        }
    }

    public Box Create()
    { 
        return CreateInternal();
    }

    public Box CreateAt(Vector3 position)
    {
        Box box = CreateInternal();
        box.transform.position = position;
        return box;
    }
}
