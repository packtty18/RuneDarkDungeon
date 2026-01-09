using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

[Serializable]
public class AttackTypeConfig
{
    [Tooltip("공격 타입")]
    public EAttackType AttackType;

    [Tooltip("공격 단계 목록 (콤보 또는 단일 공격)")]
    public AttackPhaseData[] AttackPhases;

    [Tooltip("데미지")]
    public float Damage;

    [Tooltip("연속(콤보) 공격 가능 여부")]
    public bool IsComboAttack => AttackPhases.Length > 1;

    [Tooltip("최대 연속 공격 수")]
    public int MaxPhaseCount => AttackPhases?.Length ?? 0;

    public AttackPhaseData GetPhaseData(int phaseIndex)
    {
        if (AttackPhases == null || phaseIndex <= 0 || phaseIndex > MaxPhaseCount)
        {
            return null;
        }

        return AttackPhases[phaseIndex - 1];
    }
}
