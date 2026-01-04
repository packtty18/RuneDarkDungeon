using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Pool;

//풀링하고자하는 컴포넌트는 PoolableObject를 상속받아야함
[Serializable]
public class PoolConfig
{
    [Tooltip("풀링할 컴포넌트")]
    public PoolableObject Component;

    [Tooltip("풀 키 (비어있다면 프리팹 이름)")]
    public string PoolKey = "";

    [Tooltip("초기 생성 개수")]
    public int DefaultCapacity = 10;

    [Tooltip("최대 생성 개수")]
    public int MaxSize = 100;

    [Tooltip("사전 생성 여부")]
    public bool WarmUp = true;

    public string GetPoolKey() => string.IsNullOrEmpty(PoolKey) ? Component.gameObject.name : PoolKey;
}

public class PoolManager : GlobalSingleton<PoolManager>
{
    [Header("풀 설정")]
    [SerializeField] private List<PoolConfig> _poolConfigs = new List<PoolConfig>();

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

        CreatPoolsFromConfigs();
    }

    private void CreatPoolsFromConfigs()
    {
        foreach ( var config in _poolConfigs)
        {
            if (config.Component == null)
            {
                Debug.LogWarning("[PoolManager] 컴포넌트가 null인 설정이 있습니다.");
                continue;
            }

            string poolKey = config.GetPoolKey();
            Type type = config.Component.GetType();

            //매서드 찾기
            var method = GetType().GetMethod(nameof(CreatePool), BindingFlags.NonPublic | BindingFlags.Instance);
            //제너릭 타입 지정
            var genericMethod = method.MakeGenericMethod(type);

            //메서드 호출
            try
            {
                genericMethod.Invoke(this, new object[]
                {
                        poolKey,
                        config.Component,
                        config.DefaultCapacity,
                        config.MaxSize,
                        config.WarmUp
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"[PoolManager] '{poolKey}' 풀 생성 실패: {e.Message}");
            }
        }
    }

    /// <summary>
    /// 풀을 생성합니다.
    /// </summary>
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

    /// <summary>
    /// 풀에서 오브젝트 가져오기
    /// </summary>
    public T Get<T> (string key) where T : PoolableObject
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            var obj = pool.Get();
            if (obj is T typed)
            {
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

    /// <summary>
    /// 키로 풀로 반환 (PoolableObject용)
    /// </summary>
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

    public void Clear(String key)
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

    /// <summary>
    /// 풀 통계 정보
    /// </summary>
    public PoolStats GetStats(string key)
    {
        if (_pools.TryGetValue(key, out var pool))
        {
            return pool.GetStats();
        }
      
        Debug.LogError($"[PoolManager] '{key}' 풀을 찾을 수 없습니다.");
        return null;
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