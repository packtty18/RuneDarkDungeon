using UnityEngine;

public class ChargeState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Charge;

    private Vector3 _direction;
    private Vector3 _startPosition;
    private int _chargeCount;

    [Header("Wall Detection")]
    private float _wallCheckDistance = 1.0f;
    private LayerMask _obstacleLayer;

    public ChargeState(EnemyController controller)
        : base(controller) 
    {

        _obstacleLayer = LayerMask.GetMask("Wall", "Obstacle", "Default");
    }

    public override void Enter()
    {
        base.Enter();
        SetDestination();

        controller.Move.SetAbleToRatate(false);
        controller.Anim.SetBool(EnemyAnimator.s_moveBool, true);
        controller.Anim.SetBool(EnemyAnimator.s_chargeBool, true);

        if (controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.EnableSuperArmor();
        }

        controller.Attack.StartCharge();
        _chargeCount = 0;
        Debug.Log("[ChargeState] Enter - 돌진 시작");
    }

    private void SetDestination()
    {
        _startPosition = controller.transform.position;
        _direction = (controller.Target.position - _startPosition).normalized;
        _direction.y = 0f; // Y축 고정
    }

    public override void Tick(float deltaTime)
    {
        float chargeSpeed = controller.Stat.GetValue(EEnemyValueFloat.ChargeSpeed).Value;
        float moveStep = chargeSpeed * deltaTime;

        if (DetectWallAhead(moveStep))
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        Vector3 nextPosition = controller.transform.position + _direction * moveStep;
        if (IsOutOfBounds(nextPosition))
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
            return;
        }

        controller.Move.MoveByDirection(_direction, chargeSpeed);

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

    private bool DetectWallAhead(float moveStep)
    {
        Vector3 origin = controller.transform.position + Vector3.up * 0.5f; 
        float checkDistance = moveStep + _wallCheckDistance;

        if (Physics.Raycast(origin, _direction, out RaycastHit hit, checkDistance, _obstacleLayer))
        {
            Debug.DrawRay(origin, _direction * checkDistance, Color.red, 0.1f);
            return true;
        }

        Vector3 leftDir = Quaternion.Euler(0, -30, 0) * _direction;
        Vector3 rightDir = Quaternion.Euler(0, 30, 0) * _direction;

        if (Physics.Raycast(origin, leftDir, checkDistance * 0.7f, _obstacleLayer) ||
            Physics.Raycast(origin, rightDir, checkDistance * 0.7f, _obstacleLayer))
        {
            return true;
        }

        Debug.DrawRay(origin, _direction * checkDistance, Color.green, 0.1f);
        return false;
    }

    private bool IsOutOfBounds(Vector3 position)
    {
        UnityEngine.AI.NavMeshHit navHit;
        float maxDistance = 2.0f;

        if (UnityEngine.AI.NavMesh.SamplePosition(position, out navHit, maxDistance, UnityEngine.AI.NavMesh.AllAreas))
        {
            float distance = Vector3.Distance(position, navHit.position);
            return distance > 1.5f;
        }
        return true;
    }

    public override void Exit()
    {
        if (controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.DisableSuperArmor();
        }

        controller.Anim.SetBool(EnemyAnimator.s_moveBool, false);
        controller.Anim.SetBool(EnemyAnimator.s_chargeBool, false);
        controller.Attack.EndCharge();
        
        Debug.Log("[ChargeState] Exit → Chase");
    }
}
