public class DeadState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Dead;

    public DeadState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.PauseAgent();
        controller.Move.SetAbleToRatate(false);

        if (controller.Stat.EnemyType == EEnemyType.Boss)
        {
            BossAttack attack = controller.Attack as BossAttack;
            attack.TargetSpawner.KillAll();
        }


        controller.CancelAttack();
        controller.Anim.SetTrigger(EnemyAnimator.s_deadTrigger);
        
        controller.Sound?.PlayDeath();
        
        controller.Dead();
    }

    public override void Tick(float deltaTime)
    {
    }
}