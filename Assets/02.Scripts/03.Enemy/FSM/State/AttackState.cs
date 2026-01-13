using UnityEngine;

public class AttackState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Attack;

    private float cooldownTimer;
    private bool isAttacking;

    public AttackState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        isAttacking = false;
        cooldownTimer = 0f;
    }

    public override void Tick(float deltaTime)
    {
        if (isAttacking)
        {
            return;
        }

        cooldownTimer -= deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        float range = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (!controller.IsTargetInRange(range))
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        int attackId = controller.Attack.GetRandomAttackId();
        if (!controller.Attack.Execute(attackId))
        {
            return;
        }

        isAttacking = true;
        cooldownTimer = controller.Stat.GetValue(EEnemyValueFloat.AttackCoolDown).Value;

        controller.Anim.SetInt(AnimatorController.s_attackIdInt, attackId);
        controller.Anim.SetTrigger(AnimatorController.s_attackTrigger);
    }

    public override void Exit()
    {
        isAttacking = false;
        controller.Attack.Finish();
    }

    public void OnAttackFinished()
    {
        isAttacking = false;
    }
}
