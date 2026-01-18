using UnityEngine;

/// <summary>
/// 방법 2: Rigidbody를 사용한 물리 기반 돌진
/// 벽 충돌 시 자동으로 멈추며, 더 현실적인 동작
/// </summary>
public class ChargeState_PhysicsBased : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Charge;

    private Vector3 _direction;
    private Vector3 _startPosition;
    private int _chargeCount;
    private Rigidbody _rigidbody;

    private float _chargeTimer = 0f;
    private float _maxChargeTime = 3f; // 최대 돌진 시간 (안전장치)

    public ChargeState_PhysicsBased(EnemyController controller)
        : base(controller) 
    {
        _rigidbody = controller.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        base.Enter();
        SetDestination();

        // Rigidbody 활성화
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = false; // 돌진 중 중력 비활성화
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }

        controller.Move.SetAbleToRatate(false);
        controller.Anim.SetBool(EnemyAnimator.s_moveBool, true);
        controller.Anim.SetBool(EnemyAnimator.s_chargeBool, true);

        if (controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.EnableSuperArmor();
        }

        controller.Attack.StartCharge();
        _chargeCount = 0;
        _chargeTimer = 0f;
        
        Debug.Log("[ChargeState] Enter - 물리 기반 돌진 시작");
    }

    private void SetDestination()
    {
        _startPosition = controller.transform.position;
        _direction = (controller.Target.position - _startPosition).normalized;
        _direction.y = 0f;
        _chargeTimer = 0f;
    }

    public override void Tick(float deltaTime)
    {
        _chargeTimer += deltaTime;

        // 최대 시간 초과 시 강제 종료 (안전장치)
        if (_chargeTimer > _maxChargeTime)
        {
            Debug.Log("[ChargeState] 최대 시간 초과 - 돌진 중단");
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        // Rigidbody로 이동
        if (_rigidbody != null)
        {
            float chargeSpeed = controller.Stat.GetValue(EEnemyValueFloat.ChargeSpeed).Value;
            Vector3 velocity = _direction * chargeSpeed;
            velocity.y = _rigidbody.linearVelocity.y; // Y축 속도 유지
            _rigidbody.linearVelocity = velocity;
        }

        // 목표 거리 도달 체크
        float movedDistance = Vector3.Distance(_startPosition, controller.transform.position);
        if (movedDistance >= controller.Stat.GetValue(EEnemyValueFloat.ChargeDistance).Value)
        {
            _chargeCount++;

            bool isPhase3 = controller.Phase != null && controller.Phase.CurrentPhaseIndex >= 2;
            if (controller.Stat.EnemyType == EEnemyType.Boss && isPhase3 && _chargeCount < 3)
            {
                SetDestination();
                return;
            }

            controller.FSM.ChangeState(EEnemyState.Chase);
        }
    }

    public override void Exit()
    {
        // Rigidbody 정지
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.isKinematic = true;
        }

        if (controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.DisableSuperArmor();
        }

        controller.Anim.SetBool(EnemyAnimator.s_moveBool, false);
        controller.Anim.SetBool(EnemyAnimator.s_chargeBool, false);
        controller.Attack.EndCharge();
        
        Debug.Log("[ChargeState] Exit → Chase");
    }

    /// <summary>
    /// 충돌 시 돌진 중단 (EnemyController에 추가 필요)
    /// </summary>
    public void OnCollisionEnter(Collision collision)
    {
        // 벽이나 장애물과 충돌 시
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("[ChargeState] 충돌 감지 - 돌진 중단");
            controller.FSM.ChangeState(EEnemyState.Chase);
        }
    }
}
