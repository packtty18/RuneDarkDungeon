using UnityEngine;
using UnityEngine.Pool;

public class PoolWrapper<T> : IPool where T : PoolableObject
{
    private ObjectPool<T> _pool;
    private string _key;

    public PoolWrapper(string key, ObjectPool<T> pool)
    {
        _key = key;
        _pool = pool;
    }

    public void Clear()
    {
        _pool.Clear();
    }

    public PoolableObject Get()
    {
        return _pool.Get();
    }

    public PoolStats GetStats()
    {
        int total = _pool.CountAll;
        int active = _pool.CountActive;
        int inactive = total - active;

        return new PoolStats (_key, typeof(T).Name, total, active, inactive);
    }

    public void Release(PoolableObject obj)
    {
        if (obj is T typedObj)
        {
            _pool.Release(typedObj);
        }
        else
        {
            Debug.LogError($"[Pool] {_key} 타입 불일치");
            Object.Destroy(obj.gameObject);
        }
    }
}
