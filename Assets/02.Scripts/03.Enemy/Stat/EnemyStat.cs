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

    public bool CanCharge = true;
    [ShowInInspector] public bool HasSuperArmor { get; private set; }
    [ShowInInspector] public bool OnBerserk { get; private set; }

    public bool OnPhase1 { get; private set; }
    public bool OnPhase2 { get; private set; }
    public bool OnPhase3 { get; private set; }

    public virtual void Init()
    {
        HasSuperArmor = false;
        OnBerserk = false; 
        OnPhase1 = true;
        OnPhase2 = false;
        OnPhase3 = false;


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
        _floatValues[EEnemyValueFloat.ChargeCooldown].Init(data.chargeCooldown);
        _floatValues[EEnemyValueFloat.ChargeRange].Init(data.chargeRange);
        _floatValues[EEnemyValueFloat.ChargeSpeed].Init(data.chargeSpeed);
        _floatValues[EEnemyValueFloat.ChargeDistance].Init(data.chargeDistance);
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
    public void ActiveBerserk()
    {
        if (OnBerserk)
        {
            return;
        }

        OnBerserk = true;
        EnableSuperArmor();
    }


    public void ActivePhase2()
    {
        if (!OnPhase1 && OnPhase2)
        {
            return;
        }

        OnPhase2 = true;
        //_buff.ApplyPhase2();
    }

    public void ActivePhase3()
    {
        if (!OnPhase2 && OnPhase3)
        {
            return;
        }

        OnPhase3 = true;
        //_buff.ApplyPhase3();
    }
}