using System.Collections.Generic;
using UnityEngine;

public enum EEnemyType
{
    Warrior,
    Archer,
    Mage,
    EliteWarrior,
    Commander,
    Boss
}
public enum EEnemyValueFloat
{
    MoveSpeed,
    AttackRange
}

public enum EEnemyValueInt
{
    Attack,
    Defense
}

public enum EEnemyConsumableFloat
{
    Health
}

//public enum EEnemyConsumableInt
//{
    
//}

//데이터를 통해 Stat을 초기화 및 전달. 변경은 여기서 하지 않음
public class EnemyStat : MonoBehaviour
{
    [SerializeField] private MonsterDataSO _data;

    private readonly Dictionary<EEnemyConsumableFloat, ConsumableStat<float>> _floatConsumables = new();
    private readonly Dictionary<EEnemyValueFloat, ValueStat<float>> _floatValues = new();
    private readonly Dictionary<EEnemyValueInt, ValueStat<int>> _intValues = new();

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

        foreach (EEnemyValueInt type in System.Enum.GetValues(typeof(EEnemyValueInt)))
        {
            _intValues[type] = new ValueStat<int>();
        }
    }

    private void InitFromData(MonsterDataSO data)
    {
        _floatConsumables[EEnemyConsumableFloat.Health].Init(data.maxHP);

        _intValues[EEnemyValueInt.Attack].Init(data.attack);
        _intValues[EEnemyValueInt.Defense].Init(data.defense);

        _floatValues[EEnemyValueFloat.MoveSpeed].Init(data.moveSpeed);
        _floatValues[EEnemyValueFloat.AttackRange].Init(data.attackRange);

        OnStatInitEnd?.Invoke();

        Debug.Log("[EnemyStat] Initialized");
    }

    public IReadOnlyValue<float> GetValue(EEnemyValueFloat type)
    {
        return _floatValues[type];
    }

    public IReadOnlyValue<int> GetValue(EEnemyValueInt type)
    {
        return _intValues[type];
    }

    public IReadOnlyConsumable<float> GetValue(EEnemyConsumableFloat type)
    {
        return _floatConsumables[type];
    }
}

