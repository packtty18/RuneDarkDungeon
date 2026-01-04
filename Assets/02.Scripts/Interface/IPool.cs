using UnityEngine;

public interface IPool 
{
    PoolableObject Get();
    void Release(PoolableObject obj);

    void Clear();

    PoolStats GetStats();
}
