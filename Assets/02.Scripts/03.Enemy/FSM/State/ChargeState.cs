using UnityEngine;

public class ChargeState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Charge;

    private Vector3 _direction;
    private Vector3 _startPosition;
    private int _chargeCount;
    public ChargeState(EnemyController controller)
        : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        SetDestination();

        controller.EnablePhysics(false);

        controller.Move.PauseAgent();
        controller.Anim.SetBool("IsMove", true);
        controller.Anim.SetBool("IsCharge", true);

        if (controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.EnableSuperArmor();
        }

        controller.Attack.StartCharge();
        _chargeCount = 0;
        Debug.Log("[DashState] Enter");
    }

    private void SetDestination()
    {
        _startPosition = controller.transform.position;
        _direction = (controller.Target.position - _startPosition).normalized;
    }

    public override void Tick(float deltaTime)
    {
        
        controller.Move.MoveByDirection(_direction, controller.Stat.GetValue(EEnemyValueFloat.ChargeSpeed).Value);

        float movedDistance = Vector3.Distance(_startPosition, controller.transform.position);
        if (movedDistance >= controller.Stat.GetValue(EEnemyValueFloat.ChargeDistance).Value)
        {
            _chargeCount++;

            if(controller.Stat.EnemyType == EEnemyType.Boss && controller.Stat.OnPhase3 && _chargeCount < 3)
            {
                SetDestination();
                return;
            }

            controller.FSM.ChangeState(EEnemyState.Chase);
        }
    }

    public override void Exit()
    {
        if(controller.Stat.EnemyType != EEnemyType.Boss)
        {
            controller.Stat.DisableSuperArmor();
        }
        
        controller.Anim.SetBool("IsMove", false);
        controller.Anim.SetBool("IsCharge", false);
        controller.Move.ResumeAgent();
        controller.EnablePhysics(true);
        controller.Attack.EndCharge();
        Debug.Log("[DashState] Exit → Chase");
    }
}
