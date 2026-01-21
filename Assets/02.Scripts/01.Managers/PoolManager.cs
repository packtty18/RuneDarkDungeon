using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<PoolConfigSO> _poolConfigs = new List<PoolConfigSO>();

    private Dictionary<EPoolType, ObjectPool<GameObject>> _pools = new Dictionary<EPoolType, ObjectPool<GameObject>>(); //키, Pool
    private Dictionary<EPoolType, Transform> _poolParents = new Dictionary<EPoolType, Transform>();

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
        canvas.sortingOrder = 0; // 필요에 따라 조정

        var canvasScaler = canvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080); // 프로젝트 기준 해상도로 설정이 필요할 수 있습니다.

        canvasObject.AddComponent<GraphicRaycaster>();
    }

    public void CreatePoolsFromConfigs(List<PoolConfigSO> configs)
    {
        foreach ( var config in configs)
        {
            CreatePoolFromConfig(config);
        }

    }

    public void CreatePoolFromConfig(PoolConfigSO config)
    {
        config.CreatePool(this);
    }


    public void CreatePool(
            EPoolType type,
            GameObject prefab,
            int defaultCapacity = 10,
            int maxSize = 100,
            bool warmUp = true,
            bool isUI = false)
    {
        if (prefab == null)
        {
            Debug.LogError($"[PoolManager] '{type}' 풀 생성 실패: 프리팹이 null 입니다.");
            return;
        }

        if (_pools.ContainsKey(type))
        {
            Debug.LogWarning($"[PoolManager] '{type}' 풀은 이미 존재합니다.");
            return;
        }

        Transform poolParent = new GameObject($"Pool_{type}").transform;

        if (isUI)
        {
            poolParent.SetParent(_canvas);
        }
        else
        {
            poolParent.SetParent(_poolParent);
        }

        _poolParents.Add(type, poolParent);

        var pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, poolParent, type),
                actionOnGet: OnGetFromPool,
                actionOnRelease: OnReleaseToPool,
                actionOnDestroy: OnDestroyPoolObject,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
        );
        
        _pools.Add(type, pool);


        if (warmUp)
        {
            WarmUpPool(pool, defaultCapacity);
        }

    }
    public void ReturnAll(EPoolType type)
    {
        if (!_poolParents.TryGetValue(type, out var parent))
        {
            Debug.LogWarning($"[PoolManager] '{type}' 풀을 찾을 수 없습니다.");
            return;
        }

        List<GameObject> activeObjects = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
            {
                activeObjects.Add(child.gameObject);
            }
        }

        foreach (var obj in activeObjects)
        {
            ReleaseByKey(type, obj);
        }
    }
    public void ReturnAll()
    {
        foreach (var type in _poolParents.Keys.ToArray())
        {
            ReturnAll(type);
        }
    }

    public GameObject Get (EPoolType type)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            var obj = pool.Get();
            return obj;
        }
        else
        {
            Debug.LogError($"[PoolManager] '{type}' 풀을 찾을 수 없습니다.");
            return null;
        }
    }


    internal void ReleaseByKey(EPoolType type, GameObject obj)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            pool.Release(obj);
            return;
        }

        Debug.LogWarning($"[PoolManager] '{type}' 풀을 찾을 수 없습니다. 오브젝트를 파괴합니다.");
        Destroy(obj.gameObject);
    }

    public void Clear(EPoolType type)
    {
        if (_pools.TryGetValue(type, out var pool))
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


    public PoolStats GetStats(EPoolType type)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            PoolStats stats = new PoolStats(
                poolType: type,
                totalCount: pool.CountAll,
                activeCount: pool.CountActive,
                inactiveCount: pool.CountInactive
            );
            return stats;
        }
      
        Debug.LogError($"[PoolManager] '{type}' 풀을 찾을 수 없습니다.");
        return null;
    }

    public bool HasPool(EPoolType type)
    {
        return _pools.ContainsKey(type);
    }

    private GameObject CreateObject(GameObject prefab, Transform parent, EPoolType type)
    {
        GameObject obj = Instantiate(prefab, parent);
        obj.name = $"{prefab.name} (Pooled)";
        if (obj.TryGetComponent<PoolableObject>(out var poolable))
        {
            poolable.OnReturnRequested -= ReleaseByKey;
            poolable.OnReturnRequested += ReleaseByKey;
            poolable.SetPoolType(type);
        }
        return obj;
    }

    private void OnGetFromPool(GameObject obj)
    {
        if(obj == null)
        {
            return;
        }
        obj.SetActive(true);
        if (obj.TryGetComponent<PoolableObject>(out var poolable))
        {
            poolable.OnSpawn();
        }
    }

    private void OnReleaseToPool(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            if (obj.TryGetComponent<PoolableObject>(out var poolable))
            {
                poolable.OnDespawn();
            }
        } 
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        if (obj != null)
        {
            Destroy(obj);
        } 
    }

    void WarmUpPool(ObjectPool<GameObject> pool, int defaultCapacity)
    {
        GameObject [] objects = new GameObject[defaultCapacity];

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
    public EPoolType PoolType { get; }
    public int TotalCount { get; }
    public int ActiveCount { get; }
    public int InactiveCount { get; }

    public PoolStats(EPoolType poolType, int totalCount, int activeCount, int inactiveCount)
    {
        PoolType = poolType;
        TotalCount = totalCount;
        ActiveCount = activeCount;
        InactiveCount = inactiveCount;
    }
}