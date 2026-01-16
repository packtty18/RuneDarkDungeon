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
        controller.Anim.SetBool(AnimatorController.s_moveBool, true);
        controller.Anim.SetBool(AnimatorController.s_chargeBool, true);

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
        controller.Anim.SetBool(AnimatorController.s_moveBool, false);
        controller.Anim.SetBool(AnimatorController.s_chargeBool, false);
        controller.Attack.EndCharge();
        controller.EnablePhysics(true);
        Debug.Log("[DashState] Exit → Chase");
    }
}
