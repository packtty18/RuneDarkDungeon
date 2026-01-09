using UnityEngine;


// PoolFactory<T>

// 사용 방법:
// 1. PoolableObject를 상속한 타입 T를 만든다.
// 2. PoolFactory<T>를 상속한 전용 Factory를 만든다.
// 3. 생성자에서 PoolManager와 PoolKey를 전달한다.
// 4. 초기화는 T의 OnSpawn()에서 수행한다. 
// 5. 생성 시 필요한 로직은 OnCreated()를 오버라이드하여 구현한다.
// 6. 게임 로직에서 전용 Factory을 new 키워드로 생성하여 사용한다.

// 주의:
// - Factory는 MonoBehaviour가 아님.
// - new 키워드로 생성하여 사용.
// - PoolManager.Instance를 내부에서 직접 참조하지 않음.
// test/BoxFactory.cs 참고.

public abstract class PoolFactory<T> where T : MonoBehaviour
{
    protected readonly PoolManager _poolManager;
    protected readonly EPoolType _type;

    protected PoolFactory(PoolManager poolManager, EPoolType type)
    {
        _poolManager = poolManager;
        _type = type;
    }

    protected T CreateInternal()
    {
        GameObject pooledObject = _poolManager.Get(_type);
        if (pooledObject == null)
        {
            throw new System.InvalidOperationException($"PoolFactory<{typeof(T).Name}> CreateInternal Error: Failed to get an object from pool '{_type}'.");
        }

        T component = pooledObject.GetComponent<T>();
        if (component == null)
        {
            throw new System.InvalidOperationException($"PoolFactory<{typeof(T).Name}> CreateInternal Error: Pooled object is not of type '{typeof(T).Name}'.");
        }
        OnCreated(component);
        return component;
    }

    protected virtual void OnCreated(T obj) { }
}
