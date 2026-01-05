using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;



// PoolManager

// 사용 방법:
// 
// 1. PoolableObject를 상속한 컴포넌트를 만든다 (예: Bullet, Enemy).
// 2. PoolConfig<T>를 상속한 클래스를 만들어 컴포넌트와 풀 설정을 정의한다.
// 3-1. 전역풀 사용 시 PoolManager의 _poolConfigs 리스트에 PoolConfig를 추가. (전역풀 사용 권장)
// 3-2. 지역풀 사용 시 각 씬에서 PoolManager.CreatePoolFromConfig()나 list로 받아 .CreatePoolsFromConfigs() 호출로 풀 생성. 
// 4. PoolManager의 _poolConfigs 리스트에 PoolConfig를 추가하면 Awake 시 자동으로 풀 생성.
//    (또는 런타임에 PoolManager.CreatePoolFromConfig() 호출로 동적 생성 가능) (Factory 패턴 권장) 
// 5. 게임 로직에서 PoolManager.Instance.Get<T>("PoolKey")를 호출하여 오브젝트를 가져온다. (Factory 패턴 권장)  
// 6. 사용이 끝나면 PoolableObject.ReturnToPool() 호출.
// test/BoxPoolConfig 참고.


public class PoolManager : GlobalSingleton<PoolManager>
{
    [Header("풀 설정")]
    [SerializeField] private List<PoolConfigBase> _poolConfigs = new List<PoolConfigBase>();

    private Dictionary<string, IPool> _pools = new Dictionary<string, IPool>(); //키, IPool
    
    private Transform _poolParent;
    private Transform _canvas;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnInit()
    {
        _poolParent = new GameObject("PoolParent").transform;
        _poolParent.SetParent(transform);
        GameObject canvasObject = new GameObject("PoolCanvas");
        canvasObject.transform.SetParent(_poolParent);
        
        CanvasSetting(canvasObject);

        _canvas = canvasObject.transform;
        CreatePoolsFromConfigs(_poolConfigs);
    }
    
    private void CanvasSetting(GameObject canvasObject)
    {
        Canvas canvas = canvasObject.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100; // 필요에 따라 조정

        var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080); // 프로젝트 기준 해상도로 설정이 필요할 수 있습니다.

        canvasObject.AddComponent<GraphicRaycaster>();
    }

    public void CreatePoolsFromConfigs(List<PoolConfigBase> configs)
    {
        foreach ( var config in configs)
        {
            CreatePoolFromConfig(config);
        }

    }

    public void CreatePoolFromConfig(PoolConfigBase config)
    {
        config.CreatePool(this);
    }


    public void CreatePool<T>(
            string key,
            T component,
            int defaultCapacity = 10,
            int maxSize = 100,
            bool warmUp = true,
            bool isUI = false) where T : PoolableObject
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

        if (isUI)
        {
            poolParent.SetParent(_canvas);
        }
        else
        {
            poolParent.SetParent(_poolParent);
        }
          
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