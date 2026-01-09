using UnityEngine;

public class AttackState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Attack;

    public AttackState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        // 공격 실행 중이면 기다림
        if (controller.Attack.IsAttacking)
        {
            return;
        }

        //타겟이 없다면 Idle
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        // 사거리 벗어나면 Chase
        if (!controller.Attack.IsTargetInRange())
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        // 아직 쿨타임이면 여기서 가만히 대기
        if (!controller.Attack.CanAttack())
        {
            return;
        }

        // 랜덤 공격 실행
        int randomID = controller.Attack.GetRandomAttackID();
        if (controller.Attack.RequestAttack(randomID))
        {
            controller.Attack.StartCooldown();
            controller.Anim.SetInt(AnimatorController.s_attackIdInt, randomID);
            controller.Anim.SetTrigger(AnimatorController.s_attackTrigger);
        }
    }
}