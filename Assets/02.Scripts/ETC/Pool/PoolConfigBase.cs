using UnityEngine;

public abstract class PoolConfigBase : MonoBehaviour
{
    public abstract string PoolKey { get; }
    public abstract void CreatePool(PoolManager manager);
}
