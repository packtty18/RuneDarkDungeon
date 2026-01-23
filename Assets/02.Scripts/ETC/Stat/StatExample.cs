
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatExample : MonoBehaviour
{

    [SerializeField] private MovementDataSO _moveData;
    [SerializeField] private BombDataSO _currentBombData;
    [ SerializeField] private GunDataSO _currentGunData;

    [SerializeField]
    private Dictionary<EConsumableFloat, ConsumableStat<float>> _floatConsumableStat = new Dictionary<EConsumableFloat, ConsumableStat<float>>();
    [SerializeField]
    private Dictionary<EConsumableInt, ConsumableStat<int>> _intConsumableStat = new Dictionary<EConsumableInt, ConsumableStat<int>>();
    [SerializeField]
    private Dictionary<EValueFloat, ValueStat<float>> _floatValueStat = new Dictionary<EValueFloat, ValueStat<float>>();
    [SerializeField]
    private Dictionary<EValueInt, ValueStat<int>> _intValueStat = new Dictionary<EValueInt, ValueStat<int>>();


    public MovementDataSO CurrentMove => _moveData;
    public BombDataSO CurrentBomb => _currentBombData;
    public GunDataSO CurrentGun => _currentGunData;

    public bool IsDead { get; private set; } = false;

    public SafeEvent OnDead = new SafeEvent();
    public SafeEvent OnStatInitEnd = new SafeEvent();


    public void Awake()
    {
        OnDead ??= new SafeEvent();

        OnStatInitEnd ??= new SafeEvent();

        InitDictionaries();
        //Value 우선 처리
        _floatValueStat[EValueFloat.MaxHealth].Init(100);
        _floatValueStat[EValueFloat.HealthRegenPerSecond].Init(1);

        SetMoveData(CurrentMove);
        SetGunData(CurrentGun);
        SetBombData(CurrentBomb);

        IsDead = false;
        _floatConsumableStat[EConsumableFloat.Health].Init(
            _floatValueStat[EValueFloat.MaxHealth].Value, 
            default,
            _floatValueStat[EValueFloat.HealthRegenPerSecond].Value);

        _floatConsumableStat[EConsumableFloat.Stamina].Init(
            _floatValueStat[EValueFloat.MaxStamina].Value, 
            default,
            _floatValueStat[EValueFloat.StaminaRegenPerSecond].Value);

        _intConsumableStat[EConsumableInt.BombCount].Init(
            _intValueStat[EValueInt.BombMaxCount].Value, 
            default,
            0.2f); //폭탄 임시 리젠 => 원래는 자동 리젠 불가(아이템 획득을 통한 증가)

        _intConsumableStat[EConsumableInt.InvenBulletCount].Init(150, default); //임시 => 인벤 제작후 추가

        _intConsumableStat[EConsumableInt.LoadedBulletCount].Init(_intValueStat[EValueInt.GunMaxBullet].Value, default);
        OnStatInitEnd?.Invoke();
        //Debug.Log("[PlayerStat] Initialized");
    }

    private void InitDictionaries()
    {
        foreach (EConsumableFloat type in Enum.GetValues(typeof(EConsumableFloat)))
        {
            _floatConsumableStat[type] = new ConsumableStat<float>();
        }

        foreach (EConsumableInt type in Enum.GetValues(typeof(EConsumableInt)))
        {
            _intConsumableStat[type] = new ConsumableStat<int>();
        }

        foreach (EValueFloat type in Enum.GetValues(typeof(EValueFloat)))
        {
            _floatValueStat[type] = new ValueStat<float>();
        }

        foreach (EValueInt type in Enum.GetValues(typeof(EValueInt)))
        {
            _intValueStat[type] = new ValueStat<int>();
        }
    }


    private void SetMoveData(MovementDataSO data)
    {
        _floatValueStat[EValueFloat.MaxStamina].Init(data.MaxStamina);
        _floatValueStat[EValueFloat.StaminaRegenPerSecond].Init(data.StaminaRegen);
        _floatValueStat[EValueFloat.StaminaRegenDelay].Init(data.RegenDelay);
        _floatValueStat[EValueFloat.DashConsumeStaminaPerSecond].Init(data.DashConsume);
        _floatValueStat[EValueFloat.DoubleJumpConsumeStaminaPerOnce].Init(data.DoubleJumpConsume);
        _floatValueStat[EValueFloat.MoveSpeed].Init(data.MoveSpeed);
        _floatValueStat[EValueFloat.DashSpeed].Init(data.DashSpeed);
        _floatValueStat[EValueFloat.JumpPower].Init(data.JumpPower);
    }
    

    private void SetGunData(GunDataSO data)
    {
        _intValueStat[EValueInt.GunMaxBullet].Init(data.MaxBullet);

        _floatValueStat[EValueFloat.GunDamage].Init(data.Damage);
        _floatValueStat[EValueFloat.GunFireDelay].Init(data.FireDelay);
        _floatValueStat[EValueFloat.GunMaxRange].Init(data.Range);
        _floatValueStat[EValueFloat.GunReloadTime].Init(data.ReloadTime);

        _floatValueStat[EValueFloat.GunRecoilX].Init(data.RecoilData.XStrength);
        _floatValueStat[EValueFloat.GunRecoilY].Init(data.RecoilData.YStrength);
        _floatValueStat[EValueFloat.GunKnockbackPower].Init(data.KnockbackData.Power);
    }

    private void SetBombData(BombDataSO data)
    {
        _intValueStat[EValueInt.BombMaxCount].Init(data.MaxCount);

        _floatValueStat[EValueFloat.BombDamage].Init(data.Damage);
        _floatValueStat[EValueFloat.BombRadius].Init(data.ExplosionRadius);
        _floatValueStat[EValueFloat.BombThrowForce].Init(data.Force);
        _floatValueStat[EValueFloat.BombThrowDelay].Init(data.Delay);
        _floatValueStat[EValueFloat.BombKnockbackPower].Init(data.Knockback.Power);
    }

    public void SetDead(bool tf)
    {
        if (IsDead == tf)
            return;

        IsDead = tf;
        //Debug.Log("[PlayerStat] Dead State Changed");

        OnDead?.Invoke();
    }


    public void SetReloadBullet(int consume, int setCount)
    {
        _intConsumableStat[EConsumableInt.InvenBulletCount].Consume(consume);
        _intConsumableStat[EConsumableInt.LoadedBulletCount].SetCurrent(setCount);
    }

    public IReadOnlyConsumable<float> GetConsumable(EConsumableFloat type)
    {
        if(_floatConsumableStat.Count == 0)
        {
            return null;
        }
        return _floatConsumableStat[type];
    }

    public IReadOnlyConsumable<int> GetConsumable(EConsumableInt type)
    {
        if (_intConsumableStat.Count == 0)
        {
            return null;
        }
        return _intConsumableStat[type];
    }

    public IReadOnlyValue<float> GetValue(EValueFloat type)
    {
        if (_floatValueStat.Count == 0)
        {
            return null;
        }
        return _floatValueStat[type];
    }

    public IReadOnlyValue<int> GetValue(EValueInt type)
    {
        if (_intValueStat.Count == 0)
        {
            return null;
        }
        return _intValueStat[type];
    }
}
