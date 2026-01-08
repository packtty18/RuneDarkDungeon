using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawnManager : SerializedMonoBehaviour
{
    [SerializeField] private List<EnemySpawnerDataSO> _phase = new();
    [SerializeField] private Dictionary<EEnemyType, List<EnemySpawner>> _spawners = new();

    [Header("Phase")]
    [SerializeField] private int _nextPhaseThreshold = 0;

    public UnityEvent OnAllPhaseCompleted;

    private int _currentPhase = 0;
    private int _aliveEnemyCount = 0;


    [SerializeField] Transform target;
    private List<EnemyController> _list = new List<EnemyController>();
    [Button]
    public void PlayCurrentPhase()
    {
        _list.Clear();
        if (_currentPhase >= _phase.Count)
        {
            Debug.Log("[SpawnManager] All phases completed");
            OnAllPhaseCompleted?.Invoke();
            return;
        }

        Debug.Log($"[SpawnManager] Phase {_currentPhase} Start");

        foreach (EnemySpawnData data in _phase[_currentPhase].Datas)
        {
            SpawnEnemies(data.Type, data.Count);
        }
    }

    private void SpawnEnemies(EEnemyType type, int count)
    {
        if (!_spawners.TryGetValue(type, out var spawnerList) || spawnerList.Count == 0)
        {
            Debug.LogWarning($"해당 타입의 스포너가 없음: {type}");
            return;
        }
        
        for (int i = 0; i < count; i++)
        {
            EnemySpawner spawner =
                spawnerList[Random.Range(0, spawnerList.Count)];

            EnemyController enemy =
                spawner.Spawn(TypeConverter(type));

            if (enemy == null)
                continue;

            _aliveEnemyCount++;

            enemy.OnDead.Subscribe(HandleEnemyDead);
            enemy.SetTarget(target);
            enemy.Init();
            _list.Add(enemy);
        }

        Debug.Log($"[SpawnManager] Alive Enemy Count : {_aliveEnemyCount}");
    }

    [Button]
    private void KillAll()
    {
        if (_list == null || _list.Count == 0) return;

        for (int i = _list.Count - 1; i >= 0; i--)
        {
            _list[i].Dead();
        }
    }

    private void HandleEnemyDead(EnemyController enemy)
    {
        enemy.OnDead.Unsubscribe(HandleEnemyDead);
        _aliveEnemyCount--;

        Debug.Log($"[SpawnManager] Enemy Dead, Alive : {_aliveEnemyCount}");

        if (_aliveEnemyCount <= _nextPhaseThreshold)
        {
            _currentPhase++;
            PlayCurrentPhase();
        }
    }

    private EPoolType TypeConverter(EEnemyType type)
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
}
