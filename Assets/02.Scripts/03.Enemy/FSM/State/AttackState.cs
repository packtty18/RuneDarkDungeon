using UnityEngine;

public class AttackState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Attack;

    private float _attackCooldown = 0f;
    private float _attackDelay;

    public AttackState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        _attackCooldown = 0f; // 상태 시작 시 딜레이 초기화
        _attackDelay = controller.Stat.GetValue(EEnemyValueFloat.AttackCoolDown).Value;
    }

    public override void Update()
    {
        // 공격 실행 중이면 기다림
        if (controller.Attack.IsAttacking)
        {
            return;
        }

        // 공격 딜레이 타이머 갱신
        if (_attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
            return;
        }

        // 타겟이 없다면 Idle로
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        float attackRange = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;

        // 타겟이 공격범위를 벗어난다면 Chase로
        if (!controller.IsTargetInRange(attackRange))
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        // 랜덤 공격 실행
        int randomID = controller.Attack.GetRandomAttackID();
        if (!controller.Attack.RequestAttack(randomID))
        {
            return;
        }

        controller.Anim.SetInt(AnimatorController.s_attackIdInt, randomID);
        controller.Anim.SetTrigger(AnimatorController.s_attackTrigger);


        // 공격 후 딜레이 적용
        _attackCooldown = _attackDelay;
    }
}