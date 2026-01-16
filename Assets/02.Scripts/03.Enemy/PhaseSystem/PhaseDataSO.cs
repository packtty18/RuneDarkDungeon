using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum EPhaseTransitionType
{
    None,           // 조건 없음 (첫 Phase)
    HealthRatio,    // 체력 비율
}

[CreateAssetMenu(fileName = "PhaseData", menuName = "SO/Enemy/PhaseData", order = 0)]
public class PhaseDataSO : ScriptableObject
{
    [Title("Phase 기본 정보")]
    [SerializeField] private int _phaseIndex;
    [SerializeField] private string _phaseName;

    [Title("Phase 전환 조건")]
    [SerializeField] private EPhaseTransitionType _transitionType;

    [SerializeField, ShowIf(nameof(_transitionType), EPhaseTransitionType.HealthRatio)]
    [Range(0f, 1f), LabelText("체력 비율 (0~1)")]
    private float _healthRatioThreshold = 0.5f;

    [Title("Phase 진입 시 적용 효과")]
    [SerializeField, LabelText("적용할 버프")]
    private List<BuffSO> _buffsToApply = new List<BuffSO>();
    [SerializeField, LabelText("공격 패턴 변경")]
    private bool _changeAttackPattern;

    public int PhaseIndex => _phaseIndex;
    public string PhaseName => _phaseName;
    public EPhaseTransitionType TransitionType => _transitionType;
    public float HealthRatioThreshold => _healthRatioThreshold;
    public List<BuffSO> BuffsToApply => _buffsToApply;
    public bool ChangeAttackPattern => _changeAttackPattern;

    public bool CheckTransition(EnemyController controller)
    {
        return _transitionType switch
        {
            EPhaseTransitionType.None => false,
            EPhaseTransitionType.HealthRatio => CheckHealthRatio(controller),
            _ => false
        };
    }

    private bool CheckHealthRatio(EnemyController controller)
    {
        float currentRatio = controller.Stat.GetValue(EEnemyConsumableFloat.Health).GetRatio();
        return currentRatio <= _healthRatioThreshold;
    }
}