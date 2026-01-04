using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/*
 * PoolManager
 *
 * 사용 방법:
 * 1. PoolableObject를 상속한 컴포넌트를 만든다 (예: Bullet, Enemy).
 * 2. 인스펙터에서 PoolManager를 붙인 게임오브젝트를 만든다.
 * 3. PoolConfig<T>를 상속한 클래스를 만들어 프리팹과 풀 설정을 정의한다.
 *    - 예: PoolConfig<Bullet> bulletPoolConfig
 * 4. PoolManager의 _poolConfigs 리스트에 PoolConfig를 추가하면 Awake 시 자동으로 풀 생성.
 * 5. 게임 로직에서 PoolManager.Instance.Get<T>("PoolKey")를 호출하여 오브젝트를 가져온다. (Factory 패턴 권장)  
 * 6. 사용이 끝나면 PoolableObject.ReturnToPool() 호출.
 * test/BoxPoolConfig 참고.
 *
 */
[Serializable]
public class PoolConfig<T> : PoolConfigBase where T : PoolableObject
{
    [SerializeField] private string _poolKey;
    [SerializeField] private T _component;
    [SerializeField] private int _defaultCapacity =10;
    [SerializeField] private int _maxSize = 100;
    [SerializeField] private bool _warmUp = true;

    public override string PoolKey => _poolKey;

    public string GetPoolKey() => string.IsNullOrEmpty(PoolKey) ? _component.gameObject.name : PoolKey;
    public override void CreatePool(PoolManager manager)
    {
        manager.CreatePool<T>(GetPoolKey(), _component, _defaultCapacity, _maxSize, _warmUp);
    }
}

public class PoolManager : GlobalSingleton<PoolManager>
{
    [Header("풀 설정")]
    [SerializeField] private List<PoolConfigBase> _poolConfigs = new List<PoolConfigBase>();

    private Dictionary<string, IPool> _pools = new Dictionary<string, IPool>(); //키, IPool
    
    private Transform _poolParent;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        _poolParent = new GameObject("PoolParent").transform;
        _poolParent.SetParent(transform);

        CreatePoolsFromConfigs();
    }

    private void CreatePoolsFromConfigs()
    {
        foreach ( var config in _poolConfigs)
        {
            config.CreatePool(this);

        }
    }


    public void CreatePool<T>(
            string key,
            T component,
            int defaultCapacity = 10,
            int maxSize = 100,
            bool warmUp = true) where T : PoolableObject
    {
        if (_pools.ContainsKey(key))
        {
            Debug.LogWarning($"[PoolManager] '{key}' 풀은 이미 존재합니다.");
            return;
        }

        if (component == null)
        {
            Debug.LogError($"[PoolManager] '{key}' 풀 생성 실패: 프리팹에 '{typeof(T).Name}' 컴포넌트가 없습니다.");
            return;
        }

        Transform poolParent = new GameObject($"Pool_{key}").transform;
        poolParent.SetParent(_poolParent);

        var pool = new ObjectPool<T>(
            createFunc: () => CreateObject(component, poolParent, key),
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
        );
        
        IPool poolWrapper = new PoolWrapper<T>(key, pool);
        _pools.Add(key, poolWrapper);


        if (warmUp)
        {
            WarmUpPool(pool, defaultCapacity);
        }

    }

    public T Get<T> (string key) where T : PoolableObject
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            var obj = pool.Get();
            if (obj is T typed)
            {
                typed.OnReturnRequested -= ReleaseByKey;
                typed.OnReturnRequested += ReleaseByKey;

                return typed;
            }

            Debug.LogError($"[PoolManager] '{key}' 풀 타입 불일치"); // 수정
            return null;
        }
        else
        {
            Debug.LogError($"[PoolManager] '{key}' 풀을 찾을 수 없습니다.");
            return null;
        }
    }


    internal void ReleaseByKey(string key, PoolableObject obj)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            pool.Release(obj);
            return;
        }

        Debug.LogWarning($"[PoolManager] '{key}' 풀을 찾을 수 없습니다. 오브젝트를 파괴합니다.");
        Destroy(obj.gameObject);
    }

    public void Clear(string key)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            pool.Clear();
        }   
    }

    public void ClearAll()
    {
        foreach (var pool in _pools.Values)
        {
            pool.Clear();
        }
    }


    public PoolStats GetStats(string key)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            return pool.GetStats();
        }
      
        Debug.LogError($"[PoolManager] '{key}' 풀을 찾을 수 없습니다.");
        return null;
    }

    public bool HasPool(string key)
    {
        return _pools.ContainsKey(key);
    }

    private T CreateObject<T>(T original, Transform parent, string poolKey) where T : PoolableObject
    {
        T obj = Instantiate(original, parent);
        obj.gameObject.name = $"{original.name} (Pooled)";
        obj.SetPoolKey(poolKey);
        return obj;
    }

    private void OnGetFromPool<T>(T obj) where T : PoolableObject
    {
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
    }

    private void OnReleaseToPool<T>(T obj) where T : PoolableObject
    {
        if (obj != null)
        {
            obj.gameObject.SetActive(false);
            obj.OnDespawn();
        } 
    }

    private void OnDestroyPoolObject<T>(T obj) where T : PoolableObject
    {
        if (obj != null)
        {
            Destroy(obj.gameObject);
        } 
    }

    void WarmUpPool<T>(ObjectPool<T> pool, int defaultCapacity) where T : PoolableObject
    {
        T[] objects = new T[defaultCapacity];

        for (int i = 0; i < defaultCapacity; i++)
        {
            objects[i] = pool.Get();
        }
        for (int i = 0; i < defaultCapacity; i++)
        {
            pool.Release(objects[i]);
        }
    }

  
}
public class PoolStats
{
    public string Key { get; }
    public string PoolType { get; }
    public int TotalCount { get; }
    public int ActiveCount { get; }
    public int InactiveCount { get; }

    public PoolStats(string key, string poolType, int totalCount, int activeCount, int inactiveCount)
    {
        Key = key;
        PoolType = poolType;
        TotalCount = totalCount;
        ActiveCount = activeCount;
        InactiveCount = inactiveCount;
    }
}