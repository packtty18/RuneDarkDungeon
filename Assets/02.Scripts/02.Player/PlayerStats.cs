using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("기본 스탯")]
    [Tooltip("체력")]
    public ConsumableStat<float> Health;
    [Tooltip("공격력")]
    public ConsumableStat<float> Attack;
    [Tooltip("방어력")]
    public ConsumableStat<float> Defense;

    [Header("공격 스탯")]
    [Tooltip("치명타 확률")]
    public ConsumableStat<float> CriticalRate;
    [Tooltip("치명타 데미지")]
    public ConsumableStat<float> CriticalDamage;


    [Header("유틸 스탯")]
    [Tooltip("쿨타임 감소량")]
    public ConsumableStat<float> CooldownReduction;
    [Tooltip("기본 이동 속도")]
    public ConsumableStat<float> MoveSpeed;
    [Tooltip("점프 파워")]
    public ValueStat<float> JumpPower;
    [Tooltip("중력")]
    public ValueStat<float> Gravity;

    [Header("생존 스탯")]
    [Tooltip("피해 흡혈")]
    public ConsumableStat<float> Vampirism;
}
