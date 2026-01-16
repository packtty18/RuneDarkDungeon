
public class SummonState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Summon;

    private float cooldownTimer;
    public SummonState(EnemyController controller) : base(controller) { }

    
    public override void Enter()
    {
        base.Enter();
        controller.Attack.StartSummon();
        BossAttack attack = controller.Attack as BossAttack;
        attack.TargetSpawner.BossSummon();
        controller.Anim.SetTrigger(EnemyAnimator.s_summonTrigger);
        cooldownTimer = 3f;
    }

    public override void Tick(float deltaTime)
    {
        cooldownTimer -= deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        controller.FSM.ChangeState(EEnemyState.Idle);
    }

    public override void Exit()
    {
        controller.Attack.EndSummon();
        base.Exit();

    }
}
