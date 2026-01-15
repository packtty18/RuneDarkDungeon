using Sirenix.OdinInspector;
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
    MoveSpeed,      //이동속도
    AttackRange,    //공격범위
    AttackCooldown, //공격딜레이
    Attack,         //공격력
    Defense,         //방어력

    ChargeSpeed,
    ChargeRange,
    ChargeDistance,
    ChargeCooldown
}

public enum EEnemyConsumableFloat
{
    Health,         //체력
}

//public enum EEnemyConsumableInt
//{
    
//}

//데이터를 통해 Stat을 초기화 및 전달.
public class EnemyStat : SerializedMonoBehaviour
{
    [SerializeField] protected MonsterDataSO _data;

    [SerializeField] protected readonly Dictionary<EEnemyConsumableFloat, ConsumableStat<float>> _floatConsumables = new();
    [SerializeField] protected readonly Dictionary<EEnemyValueFloat, ValueStat<float>> _floatValues = new();
    //private readonly Dictionary<EEnemyConsumableInt, ConsumableStat<float>> _intConsumables = new();

    public EEnemyType EnemyType => _data.enemyType;
    public SafeEvent OnStatInitEnd = new();

    public bool CanCharge = true;
    [ShowInInspector] public bool HasSuperArmor { get; private set; }

    public virtual void Init()
    {
        InitDictionaries();
        InitFromData(_data);
    }

    protected virtual void InitDictionaries()
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

    protected virtual void InitFromData(MonsterDataSO data)
    {
        _floatConsumables[EEnemyConsumableFloat.Health].Init(data.maxHP);

        _floatValues[EEnemyValueFloat.Attack].Init(data.attack);
        _floatValues[EEnemyValueFloat.Defense].Init(data.defense);
        _floatValues[EEnemyValueFloat.MoveSpeed].Init(data.moveSpeed);
        _floatValues[EEnemyValueFloat.AttackRange].Init(data.attackRange);
        _floatValues[EEnemyValueFloat.AttackCooldown].Init(data.attackCooldown);

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



    public void EnableSuperArmor()
    {
        HasSuperArmor = true;
        Debug.Log("[EnemyStat] SuperArmor ON");
    }

    public void DisableSuperArmor()
    {
        HasSuperArmor = false;
        Debug.Log("[EnemyStat] SuperArmor OFF");
    }
}