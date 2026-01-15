using Unity.VisualScripting;

public class ChaseState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Chase;

    public ChaseState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.ResumeAgent();
        
        controller.Anim.SetBool(AnimatorController.s_moveBool, true);
        controller.SetConstraintsYPosition(false);
    }

    public override void Tick(float deltaTime)
    {
        switch(controller.Stat.EnemyType)
        {
            case EEnemyType.Elite:
                EliteChase();
                break;
            case EEnemyType.Boss:
                BossChase();
                break;
            default:
                CommonChase();
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.SetConstraintsYPosition(true);
        controller.Move.StopMove();
        controller.Anim.SetBool(AnimatorController.s_moveBool, false);
    }


    public void CommonChase()
    {
        controller.Move.SetTarget(controller.Target);
        controller.Move.StartMove();

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

    public void EliteChase()
    {
        controller.Move.SetTarget(controller.Target);
        controller.Move.StartMove();

        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        float chargeRange = controller.Stat.GetValue(EEnemyValueFloat.ChargeRange).Value;
        if (controller.Stat.CanCharge && controller.IsTargetInRange(chargeRange))
        {
            controller.FSM.ChangeState(EEnemyState.Charge);
        }


        float attackRange = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (controller.IsTargetInRange(attackRange))
        {
            controller.FSM.ChangeState(EEnemyState.Attack);
        }
    }

    public void BossChase()
    {
        controller.Move.SetTarget(controller.Target);
        controller.Move.StartMove();

        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        if (controller.Stat.CanSummon)
        {
            controller.FSM.ChangeState(EEnemyState.Summon);
        }


        float chargeRange = controller.Stat.GetValue(EEnemyValueFloat.ChargeRange).Value;
        if (controller.Stat.CanCharge && controller.IsTargetInRange(chargeRange))
        {
            controller.FSM.ChangeState(EEnemyState.Charge);
        }


        float attackRange = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (controller.IsTargetInRange(attackRange))
        {
            controller.FSM.ChangeState(EEnemyState.Attack);
        }
    }
}