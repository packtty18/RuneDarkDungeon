public class DeadState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Dead;

    public DeadState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Dead();
    }

    public override void Tick(float deltaTime)
    {
    }
}