public class HitState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Hit;

    public HitState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.PauseAgent();
        controller.CancelAttack();
        controller.Sound.PlayHit();
        controller.Anim.SetTrigger(EnemyAnimator.s_hitTrigger);
    }

    public override void Tick(float deltaTime)
    {
    }
    public override void Exit()
    {
        base.Exit();
        controller.Move.ResumeAgent();
    }
}