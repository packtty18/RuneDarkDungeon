public class ChaseState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Chase;

    public ChaseState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.ResumeAgent();
        controller.Move.SetTarget(controller.Target);
        controller.Move.StartMove();
        controller.Anim.SetBool(AnimatorController.s_moveBool, true);
    }

    public override void Update()
    {
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        float attackRange = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (controller.IsTargetInRange(attackRange))
        {
            controller.FSM.ChangeState(EEnemyState.Attack);
        }

    }

    public override void Exit()
    {
        base.Exit();
        controller.Move.StopMove();
        controller.Anim.SetBool(AnimatorController.s_moveBool, false);
    }
}