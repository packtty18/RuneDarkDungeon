public class AttackState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Attack;

    private float _cooldownTimer;
    private bool _isAttacking;

    public AttackState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        _isAttacking = false;
        _cooldownTimer = 0f;
    }

    public override void Tick(float deltaTime)
    {
        if (_isAttacking)
        {
            return;
        }

        StateTransition specialTransition = controller.Behavior.UpdateAttack();
        if (specialTransition.ShouldTransition)
        {
            controller.FSM.ChangeState(specialTransition.NextState);
            return;
        }

        // 타겟 체크
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Idle);
            return;
        }

        // 범위 체크
        float range = controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (!controller.IsTargetInRange(range))
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        // 쿨다운 체크
        _cooldownTimer -= deltaTime;
        if (_cooldownTimer > 0f)
        {
            return;
        }

        // 공격 실행
        int attackId = controller.Attack.GetRandomAttackId();
        if (!controller.Attack.Execute(attackId))
        {
            return;
        }

        _isAttacking = true;
        _cooldownTimer = controller.Stat.GetValue(EEnemyValueFloat.AttackCooldown).Value;

        controller.Anim.SetInt(AnimatorController.s_attackIdInt, attackId);
        controller.Anim.SetTrigger(AnimatorController.s_attackTrigger);

        controller.Behavior.OnAttackStart();
    }

    public override void Exit()
    {
        _isAttacking = false;
        controller.Attack.Finish();
    }

    public void OnAttackFinished()
    {
        _isAttacking = false;

        controller.Behavior.OnAttackFinish();
    }
}