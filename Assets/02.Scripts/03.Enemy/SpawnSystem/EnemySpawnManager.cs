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

    //페이즈를 시작한다.
    [Button]
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

        SpawnByData(_phaseDatas[_currentPhase]);
    }

    public void BossSummon()
    {
        SpawnByData(_phaseDatas[0]);
        //WakeUpEnemies();
    }

    private void WakeUpEnemies()
    {
        foreach (EnemyController enemy in _list)
        {
            //enemy.WakeUp();

        }
    }

    private void SpawnByData(EnemyPhaseDataSO phaseDataSO)
    {
        foreach (PhaseData data in phaseDataSO.Datas)
        {
            SpawnEnemies(data.Type, data.Count, data.IsRandomSpawn);
        }
    }

    //지정된 타입에 등록된 스포너 중 랜덤한 스포너에 스폰명령 전달
    private void SpawnEnemies(EEnemyType type, int count, bool random)
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

            EnemyController enemy = spawner.SpawnEnemy(EnemyToPoolType(type));
            if (enemy == null)
            {
                continue;
            }

            _aliveEnemyCount++;

            //적설정
            enemy.OnDead.Subscribe(HandleEnemyDead);
            enemy.SetTarget(GetTarget());
            enemy.Init();

            _list.Add(enemy);
        }

        Debug.Log($"[SpawnManager] 스폰된 적 : {_aliveEnemyCount}");
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
    //테스트용 현재 스폰된 모든 적을 죽이기 => 다음 페이즈 실행
    [Button]
    private void KillAll()
    {
        if (_list == null || _list.Count == 0) return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            _list[i].HandleDead ();
        }
    }
}
