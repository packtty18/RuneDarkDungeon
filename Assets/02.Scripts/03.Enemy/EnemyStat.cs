using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Warrior,
    Archer,
    Mage,
    Elite,
    Boss
}
public enum EEnemyValueFloat
{
    MoveSpeed,
    AttackRange,
    AttackCoolDown,
    Attack,
    Defense
}

public enum EEnemyConsumableFloat
{
    Health,
}

//public enum EEnemyConsumableInt
//{
    
//}

//데이터를 통해 Stat을 초기화 및 전달.
public class EnemyStat : MonoBehaviour
{
    [SerializeField] private MonsterDataSO _data;

    private readonly Dictionary<EEnemyConsumableFloat, ConsumableStat<float>> _floatConsumables = new();
    private readonly Dictionary<EEnemyValueFloat, ValueStat<float>> _floatValues = new();
    //private readonly Dictionary<EEnemyConsumableInt, ConsumableStat<float>> _intConsumables = new();

    public EEnemyType EnemyType => _data.enemyType;
    public SafeEvent OnStatInitEnd = new();

    public void Init()
    {
        InitDictionaries();
        InitFromData(_data);
    }

    private void InitDictionaries()
    {
        foreach (EEnemyConsumableFloat type in System.Enum.GetValues(typeof(EEnemyConsumableFloat)))
        {
            _floatConsumables[type] = new ConsumableStat<float>();
        }

        foreach (EEnemyValueFloat type in System.Enum.GetValues(typeof(EEnemyValueFloat)))
        {
            _floatValues[type] = new ValueStat<float>();
        }

    }

    private void InitFromData(MonsterDataSO data)
    {
        _floatConsumables[EEnemyConsumableFloat.Health].Init(data.maxHP);

        _floatValues[EEnemyValueFloat.Attack].Init(data.attack);
        _floatValues[EEnemyValueFloat.Defense].Init(data.defense);
        _floatValues[EEnemyValueFloat.MoveSpeed].Init(data.moveSpeed);
        _floatValues[EEnemyValueFloat.AttackRange].Init(data.attackRange);
        _floatValues[EEnemyValueFloat.AttackCoolDown].Init(data.attackCooldown);
        OnStatInitEnd?.Invoke();

        Debug.Log("[EnemyStat] Initialized");
    }

    public IReadOnlyValue<float> GetValue(EEnemyValueFloat type)
    {
        return _floatValues[type];
    }

    public IReadOnlyConsumable<float> GetValue(EEnemyConsumableFloat type)
    {
        return _floatConsumables[type];
    }
}