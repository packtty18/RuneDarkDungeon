
public interface IEnemyBehavior
{
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