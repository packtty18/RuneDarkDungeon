using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Boss Phase Data")]
public class PhaseDataSO : ScriptableObject
{
    [Header("페이즈 정보")]
    public int PhaseCount;
    [Range(0f, 1f)] public float HealthRatio;
    public int ChangedAttackStat;
    public int ChangedDefenceStat;
    public float ChangedMoveSpeed;
}
