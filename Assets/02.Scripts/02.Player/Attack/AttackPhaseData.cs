using System;
using UnityEngine;
[Serializable]
public class AttackPhaseData
{
    [Header("기본 정보")]
    public int PhaseIndex;

    [Header("타이밍")]
    [Tooltip("다음 공격 입력 가능 시간")]
    public float InputWindow;

    [Header("이펙트")]
    public EffectPlayer SlashVFX;
}