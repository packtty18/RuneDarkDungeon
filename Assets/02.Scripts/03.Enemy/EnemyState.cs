using UnityEngine;

//FSM 조절
public class EnemyStateMachine
{
    private EnemyState _currentState;

    public void ChangeState(EnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Update()
    {
        _currentState?.Update();
    }
}

//베이스 스테이트
public abstract class EnemyState
{
    protected EnemyController controller;

    protected EnemyState(EnemyController controller)
    {
        this.controller = controller;
    }

    public virtual void Enter()
    {
        Debug.Log($"[FSM] Enter {GetType().Name}");
    }

    public virtual void Update() { }

    public virtual void Exit()
    {
        Debug.Log($"[FSM] Exit {GetType().Name}");
    }
}


#region Enemy States

// Idle: 스탯 초기화, 플레이어 존재 시 Chase로 전환
public class IdleState : EnemyState
{
    public IdleState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Init();
    }

    public override void Update()
    {
        if (controller.IsTargetExist())
        {
            controller.FSM.ChangeState(new ChaseState(controller));
        }
    }
}

// Chase: 플레이어 추적, 공격 범위 들어오면 Attack
public class ChaseState : EnemyState
{
    public ChaseState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.MoveToTarget();
    }

    public override void Update()
    {
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(new IdleState(controller));
            return;
        }

        float distance = Vector3.Distance(controller.transform.position, controller.GetTargetPosition());
        if (distance <= controller.Facade.Stat.GetValue(EEnemyValueFloat.AttackRange).Value)
        {
            controller.FSM.ChangeState(new AttackState(controller));
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.StopMove();
    }
}

// Attack: 플레이어 범위 내에서 랜덤 공격 실행
public class AttackState : EnemyState
{
    private float attackCooldown = 0f;
    private float AttackDelay = 3; //추후 추가할것

    public AttackState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        attackCooldown = 0f; // 상태 시작 시 딜레이 초기화
    }

    public override void Update()
    {
        // 공격 실행 중이면 기다림
        if (controller.IsOnAttack())
        {
            return;
        }

        // 공격 딜레이 타이머 갱신
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        // 타겟이 없다면 Idle로
        if (!controller.IsTargetExist())
        {
            controller.FSM.ChangeState(new IdleState(controller));
            return;
        }

        float attackRange = controller.Facade.Stat.GetValue(EEnemyValueFloat.AttackRange).Value;

        // 타겟이 공격범위를 벗어난다면 Chase로
        float distance = Vector3.Distance(controller.transform.position, controller.GetTargetPosition());
        if (distance > attackRange)
        {
            controller.FSM.ChangeState(new IdleState(controller));
            return;
        }

        // 랜덤 공격 실행
        int attackID = Random.Range(0, controller.GetAttackCount());
        controller.RequestAttack(attackID);

        // 공격 후 딜레이 적용
        attackCooldown = AttackDelay;
    }
}


// Hit: 피격, 모든 행동 취소 후 Idle로
public class HitState : EnemyState
{
    public HitState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.EnemyHitted();
    }

    public override void Update()
    {
    }
}

// Dead: 사망, 애니메이션 후 파괴
public class DeadState : EnemyState
{
    public DeadState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Dead();
    }

    public override void Update()
    {
    }
}

#endregion
