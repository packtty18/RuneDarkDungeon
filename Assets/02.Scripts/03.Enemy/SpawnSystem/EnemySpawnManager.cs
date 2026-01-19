using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


//해당 스테이지에서 페이즈별 스폰을 관리하는 주체
public class EnemySpawnManager : SerializedMonoBehaviour
{
    [SerializeField] private List<EnemyPhaseDataSO> _phaseDatas = new();
    [SerializeField] private Dictionary<EEnemyType, List<EnemySpawner>> _spawnerByType = new();

    //타입별 스폰횟수 데이터
    private Dictionary<EEnemyType, int> _spawnIndexByType = new();


    [Header("Phase")]
    [SerializeField] private int _nextPhaseThreshold = 0;

    public SafeEvent OnAllPhaseCompleted= new();

    private int _currentPhase = 0;
    private int _aliveEnemyCount = 0;

    //임시 테스트용
    [Header("Test")]
    [SerializeField] Transform _target;
    [SerializeField] private List<EnemyController> _list = new List<EnemyController>();

    public void Init()
    {
        _currentPhase = 0;
        _aliveEnemyCount = 0;
        _spawnIndexByType = new();
        _list = new List<EnemyController>();
    }

    [Button]
    //스테이지에서의 스폰
    public void SpawnCurrentPhase()
    {
        _list.Clear();
        if (_currentPhase >= _phaseDatas.Count)
        {
            Debug.Log("[SpawnManager] 모든 페이즈 실행 완료. 완료이벤트 실행");
            OnAllPhaseCompleted?.Invoke();
            return;
        }

        Debug.Log($"[SpawnManager] {_currentPhase} 페이즈 시작");

        SpawnByData(_phaseDatas[_currentPhase], true);
    }


    //보스가 스폰하는 적은 아이템을 드롭하지 않음
    public List<EnemyController> BossSummon()
    {
        SpawnByData(_phaseDatas[0], false);
        return _list;
        //WakeUpEnemies();
    }

    private void WakeUpEnemies()
    {
        foreach (EnemyController enemy in _list)
        {
            //enemy.WakeUp();

        }
    }

    private void SpawnByData(EnemyPhaseDataSO phaseDataSO, bool canItemDrop)
    {
        foreach (PhaseData data in phaseDataSO.Datas)
        {
            SpawnEnemies(data.Type, data.Count, data.IsRandomSpawn, canItemDrop);
        }
    }

    //지정된 타입에 등록된 스포너 중 랜덤한 스포너에 스폰명령 전달
    private void SpawnEnemies(EEnemyType type, int count, bool random, bool canItemDrop)
    {
        if (!_spawnerByType.TryGetValue(type, out var spawnerList) || spawnerList.Count == 0)
        {
            Debug.LogWarning($"해당 타입의 스포너가 없음: {type}");
            return;
        }

        //해당 타입이 최초로 사용된 경우
        if (!_spawnIndexByType.ContainsKey(type))
        {
            _spawnIndexByType[type] = 0;
        }

        for (int i = 0; i < count; i++)
        {
            EnemySpawner spawner = random ? spawnerList[Random.Range(0, spawnerList.Count)] : GetSequentialSpawner(type, spawnerList);

            // 1. 풀에서 꺼내기 (OnSpawn 호출됨)
            EnemyController enemy = spawner.SpawnEnemy(EnemyToPoolType(type));
            if (enemy == null)
            {
                continue;
            }

            _aliveEnemyCount++;

            // 2. 타겟 설정
            enemy.SetTarget(GetTarget());
            enemy.SetItemDrop(canItemDrop);
            // 3. 이벤트 구독
            enemy.OnDead.Subscribe(HandleEnemyDead);
            
            // 4. 초기화 (물리 엔진 활성화 포함)
            enemy.Init();
            
            Debug.Log($"[EnemySpawnManager] {type} 스폰 완료 - 최종 위치: {enemy.transform.position}");

            _list.Add(enemy);
        }

        
    }

    private Transform GetTarget()
    {
        if (BattleManager.IsExist() && BattleManager.Instance.PlayerTransform != null)
        {
            return BattleManager.Instance.PlayerTransform;
        }
        else
        {
            return _target;
        }
    }

    private EnemySpawner GetSequentialSpawner(EEnemyType type, List<EnemySpawner> spawners)
    {
        int index = _spawnIndexByType[type];
        EnemySpawner spawner = spawners[index];

        _spawnIndexByType[type] = (index + 1) % spawners.Count;
        return spawner;
    }

    private void HandleEnemyDead(EnemyController enemy)
    {
        enemy.OnDead.Unsubscribe(HandleEnemyDead);
        _aliveEnemyCount--;

        Debug.Log($"[SpawnManager] 남은 적 : {_aliveEnemyCount}");

        if (_aliveEnemyCount <= _nextPhaseThreshold)
        {
            _currentPhase++;
            SpawnCurrentPhase();
        }
    }

    //적 타입을 풀타입으로 전환
    private EPoolType EnemyToPoolType(EEnemyType type)
    {
        return type switch
        {
            EEnemyType.Warrior => EPoolType.Enemy_Warrior,
            EEnemyType.Archer => EPoolType.Enemy_Archer,
            EEnemyType.Mage => EPoolType.Enemy_Mage,
            EEnemyType.Elite => EPoolType.Enemy_Elite,
            EEnemyType.Boss => EPoolType.Enemy_Boss,
            _ => EPoolType.None
        };
    }


    [Button]
    public void KillAll()
    {
        if (_list == null || _list.Count == 0)
            return;

        // Snapshot to avoid modification during iteration
        var snapshot = new List<EnemyController>(_list);

        foreach (var enemy in snapshot)
        {
            if (enemy == null)
                continue;

            enemy.HandleDead();
        }

        Debug.Log($"[EnemySpawnManager] KillAll executed. SnapshotCount={snapshot.Count}");
    }
}
