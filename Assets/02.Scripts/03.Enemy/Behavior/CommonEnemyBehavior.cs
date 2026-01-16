public class CommonEnemyBehavior : IEnemyBehavior
{
    protected EnemyController _controller;
    public virtual void Initialize(EnemyController controller)
    {
        _controller = controller;
    }

    public virtual StateTransition UpdateChase()
    {
        _controller.Move.SetTarget(_controller.Target);
        _controller.Move.StartMove();

        if (!_controller.IsTargetExist())
        {
            return StateTransition.To(EEnemyState.Idle);
        }

        float attackRange = _controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (_controller.IsTargetInRange(attackRange))
        {
            return StateTransition.To(EEnemyState.Attack);
        }

        return StateTransition.None;
    }

    public virtual StateTransition UpdateAttack()
    {
        // 일반 적은 특수 스킬 없음
        return StateTransition.None;
    }

    public virtual bool CanCharge()
    {
        return false;
    }

    public virtual bool CanSummon()
    {
        return false;
    }

    public virtual bool CanBuff()
    {
        return false;
    }

    public virtual void OnAttackStart()
    {
    }

    public virtual void OnAttackFinish()
    {
    }
}