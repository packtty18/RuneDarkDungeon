using UnityEngine;

using UnityEngine;

/// <summary>
/// 적 타입별 행동 패턴을 정의하는 인터페이스
/// Strategy Pattern을 사용하여 타입별 로직을 캡슐화
/// </summary>
public interface IEnemyBehavior
{
    /// <summary>
    /// Behavior 초기화
    /// </summary>
    void Initialize(EnemyController controller);

    StateTransition UpdateChase();
    StateTransition UpdateAttack();

    bool CanCharge();
    bool CanSummon();
    bool CanBuff();

    void OnAttackStart();
    void OnAttackFinish();

}

public struct StateTransition
{
    public bool ShouldTransition;
    public EEnemyState NextState;

    public static StateTransition None => new StateTransition
    {
        ShouldTransition = false,
        NextState = EEnemyState.Idle
    };

    public static StateTransition To(EEnemyState state) => new StateTransition
    {
        ShouldTransition = true,
        NextState = state
    };
}