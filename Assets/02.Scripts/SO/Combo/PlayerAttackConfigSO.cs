using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttackConfigSO", menuName = "SO/Game/PlayerAttack")]
public class PlayerAttackConfigSO : ScriptableObject
{

    [Header("공격 타입별 설정")]
    [Tooltip("지상/공중 공격 설정 목록")]
    public AttackTypeConfig[] AttackConfigs;

    [Tooltip("차지 피니셔 데미지")]
    public float ChargeFinisherDamage;

    [Tooltip("차지 피니셔 홀드 타임")]
    public float ChargeTime;

    [Tooltip("스킬 시전 후 콤보 복귀 타임")]
    public float ComboReturnTime;


    public AttackTypeConfig GetAttackConfig(EAttackType attackType)
    {
        foreach(var config in AttackConfigs)
        {
            if (config.AttackType == attackType)
            {
                return config;
            }
        }

        Debug.LogWarning($"[AttackTypeConfig] {attackType} 설정을 찾을 수 없습니다.");
        return null;
    }

}
