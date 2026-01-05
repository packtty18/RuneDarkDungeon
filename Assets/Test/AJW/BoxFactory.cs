using UnityEngine;

public class BoxFactory : PoolFactory
{
    public BoxFactory(PoolManager poolManager) : base(poolManager, EPoolType.Box)
    {
        if (!_poolManager.HasPool(_type))
        {
            throw new System.InvalidOperationException($"BoxFactory Start Error: Pool with key '{_type}' does not exist.");
        }
    }

    public GameObject Create()
    { 
        return CreateInternal();
    }

    public GameObject CreateAt(Vector3 position)
    {
        GameObject box = CreateInternal();
        box.transform.position = position;
        return box;
    }
}
