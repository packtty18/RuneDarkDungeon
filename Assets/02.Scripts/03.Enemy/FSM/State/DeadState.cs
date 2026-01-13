public class DeadState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Dead;

    public DeadState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.PauseAgent();
        controller.CancelAttack();
        controller.Anim.SetTrigger(AnimatorController.s_deadTrigger);

        controller.Dead();
    }

    public override void Tick(float deltaTime)
    {
    }
}