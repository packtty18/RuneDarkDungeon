using DG.Tweening.Core.Easing;
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

public class EnemyStat : SerializedMonoBehaviour
{
    [SerializeField] protected MonsterDataSO _data;

    [SerializeField] protected readonly Dictionary<EEnemyConsumableFloat, ConsumableStat<float>> _floatConsumables = new();
    [SerializeField] protected readonly Dictionary<EEnemyValueFloat, ValueStat<float>> _floatValues = new();

    public EEnemyType EnemyType => _data.enemyType;
    public SafeEvent OnStatInitEnd = new();

    private bool _canCharge;
    private bool _canSummon;
    private bool _canBuff;

    [ShowInInspector] public bool CanCharge => _canCharge;
    [ShowInInspector] public bool CanSummon => _canSummon;
    [ShowInInspector] public bool CanBuff => _canBuff;

    [ShowInInspector] public bool HasSuperArmor { get; private set; }

    public virtual void Init()
    {
        _canCharge = true;
        _canSummon = true;
        _canBuff = true;
        HasSuperArmor = false;

        InitDictionaries();
        InitFromData(_data);

        if (EnemyType == EEnemyType.Boss)
        {
            EnableSuperArmor();
        }
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
        _floatValues[EEnemyValueFloat.ChargeCooldown].Init(data.chargeCooldown);
        _floatValues[EEnemyValueFloat.ChargeRange].Init(data.chargeRange);
        _floatValues[EEnemyValueFloat.ChargeSpeed].Init(data.chargeSpeed);
        _floatValues[EEnemyValueFloat.ChargeDistance].Init(data.chargeDistance);
        OnStatInitEnd?.Invoke();
    }

    public IReadOnlyValue<float> GetValue(EEnemyValueFloat type)
    {
        if(_floatValues.Count == 0)
        {
            return null;
        }

        return _floatValues[type];
    }

    public IReadOnlyConsumable<float> GetValue(EEnemyConsumableFloat type)
    {
        if (_floatConsumables.Count == 0)
        {
            return null;
        }
        return _floatConsumables[type];
    }



    public void EnableSuperArmor()
    {
        HasSuperArmor = true;
    }

    public void DisableSuperArmor()
    {
        HasSuperArmor = false;
    }

    public void SetActiveCharge(bool enable)
    {
        _canCharge = enable;
    }

    public void SetActiveSummon(bool enable)
    {
        _canSummon = enable;
    }

    public void SetActiveBuff(bool enable)
    {
        _canBuff = enable;
    }
}