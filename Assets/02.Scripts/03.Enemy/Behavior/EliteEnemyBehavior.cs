using UnityEngine;

/// <summary>
/// 엘리트 적의 행동 패턴
/// 일반 패턴 + 돌진 공격
/// </summary>
public class EliteEnemyBehavior : CommonEnemyBehavior
{
    public override StateTransition UpdateChase()
    {
        _controller.Move.SetTarget(_controller.Target);
        _controller.Move.StartMove();

        // 타겟이 없으면 Idle로
        if (!_controller.IsTargetExist())
        {
            return StateTransition.To(EEnemyState.Idle);
        }

        // 돌진 가능 범위 체크 (공격 범위보다 우선)
        float chargeRange = _controller.Stat.GetValue(EEnemyValueFloat.ChargeRange).Value;
        if (_controller.Stat.CanCharge && _controller.IsTargetInRange(chargeRange))
        {
            return StateTransition.To(EEnemyState.Charge);
        }

        // 일반 공격 범위 체크
        float attackRange = _controller.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;
        if (_controller.IsTargetInRange(attackRange))
        {
            return StateTransition.To(EEnemyState.Attack);
        }

        return StateTransition.None;
    }

    //페이즈 상관없이 쿨타임이 돌면 사용가능
    public override bool CanCharge()
    {
        return _controller.Stat.CanCharge;
    }

    //엘리트는 공격시 슈퍼아머 활성화
    public override void OnAttackStart()
    {
        _controller.SetActiveSuperArmor(true);
    }

    public override void OnAttackFinish()
    {
        _controller.SetActiveSuperArmor(false);
    }
}