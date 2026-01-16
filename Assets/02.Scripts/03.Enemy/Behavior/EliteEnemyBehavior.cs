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

    public override bool CanCharge()
    {
        return _controller.Stat.CanCharge;
    }

    public override void OnAttackStart()
    {
        _controller.Stat.EnableSuperArmor();
        Debug.Log("[EliteBehavior] SuperArmor 활성화");
    }

    public override void OnAttackFinish()
    {
        if (!_controller.Stat.OnBerserk)
        {
            _controller.Stat.DisableSuperArmor();
            Debug.Log("[EliteBehavior] SuperArmor 해제");
        }
        else
        {
            Debug.Log("[EliteBehavior] Berserk 상태로 SuperArmor 유지");
        }
    }
}