using UnityEngine;

[CreateAssetMenu(fileName = "PlayerComboConfigSO", menuName = "SO/Game/PlayerCombo")]
public class PlayerComboConfigSO : ScriptableObject
{

    [Header("공격 타입별 설정")]
    [Tooltip("지상/공중 공격 설정 목록")]
    public AttackTypeConfig[] AttackConfigs;

    public AttackTypeConfig GetAttackConfig(EAttackType attackType)
    {
        if (AttackConfigs == null)
        {
            return null;
        }
        foreach(var config in AttackConfigs)
        {
            if (config.AttackType == attackType)
            {
                return config;
            }
        }

        Debug.LogWarning($"[ComboConfig] {attackType} 설정을 찾을 수 없습니다.");
        return null;
    }

}
