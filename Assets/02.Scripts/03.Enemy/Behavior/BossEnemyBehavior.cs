using UnityEngine;

/// <summary>
/// 보스 적의 행동 패턴
/// 일반 패턴 + 돌진 + 소환 + 버프
/// </summary>
public class BossEnemyBehavior : EliteEnemyBehavior
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

        // 소환 우선순위가 가장 높음
        if (_controller.Stat.CanSummon)
        {
            return StateTransition.To(EEnemyState.Summon);
        }

        // 돌진 가능 범위 체크
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

    public override StateTransition UpdateAttack()
    {
        // 소환 스킬 사용 가능하면 최우선
        if (_controller.Stat.CanSummon)
        {
            return StateTransition.To(EEnemyState.Summon);
        }

        // 버프 스킬 사용 가능
        if (_controller.Stat.CanBuff)
        {
            return StateTransition.To(EEnemyState.Buff);
        }

        return StateTransition.None;
    }

    public override bool CanSummon()
    {
        return _controller.Stat.CanSummon;
    }

    public override bool CanBuff()
    {
        return _controller.Stat.CanBuff;
    }
}