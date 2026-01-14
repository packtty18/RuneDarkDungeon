using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackConfigSO", menuName = "SO/Game/PlayerAttack")]
public class PlayerAttackConfigSO : ScriptableObject
{
    [Tooltip("기본 데미지")]
    public float Damage;

    [Tooltip("점프 데쉬 데미지")]
    public float JumpDashDamage;

    [Tooltip("점프 데쉬 설정")]
    public float JumpDashAngle;
    public float JumpDashSpeed;
    public EffectPlayer JumpDashEffect;
    public EffectPlayer JumpDashSlashEffect;

    [Tooltip("차지 피니셔 데미지")]
    public float ChargeFinisherDamage;

    [Tooltip("차지 피니셔 설정")]
    public EffectPlayer FinisherEffect;
    public EffectPlayer FinisherSlashVFX;

    [Tooltip("콤보 단계 목록")]
    public AttackPhaseData[] AttackPhases;

    [Tooltip("최대 콤보 수")]
    public int MaxPhaseCount => AttackPhases?.Length ?? 0;

    [Tooltip("스킬 시전 후 콤보 복귀 타임")]
    public float ComboReturnTime;

    

    [Tooltip("차지 피니셔 홀드 타임")]
    public float ChargeTime;

    [Tooltip("점프 데쉬 콤보 연결 타임")]
    public float JumpDashComboInputWindow;

    public AttackPhaseData GetPhaseData(int phaseIndex)
    {
        if (AttackPhases == null || phaseIndex <= 0 || phaseIndex > MaxPhaseCount)
        {
            return null;
        }

        return AttackPhases[phaseIndex - 1];
    }

    public EffectPlayer[] GetComboSlashVFXs()
    {
        EffectPlayer[] list = new EffectPlayer[MaxPhaseCount];
        for (int i = 0; i < MaxPhaseCount; i++) 
        {
            list[i] = AttackPhases[i].SlashVFX;
        }

        return list;
    }
}
