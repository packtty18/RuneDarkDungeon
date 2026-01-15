using UnityEngine;

public class ChargeState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Charge;


    private Vector3 _direction;
    private Vector3 _startPosition;

    private EliteAttack _attack;
    public ChargeState(EnemyController controller)
        : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        _attack = controller.Attack as EliteAttack;

        _startPosition = controller.transform.position;
        _direction = (controller.Target.position - _startPosition).normalized;

        controller.EnablePhysics(false);

        controller.Move.PauseAgent();
        controller.Anim.SetBool("IsMove", true);
        controller.Anim.SetBool("IsCharge", true);

        controller.Stat.EnableSuperArmor();
        _attack.StartCharge();
        Debug.Log("[DashState] Enter");
    }

    public override void Tick(float deltaTime)
    {
        
        controller.Move.MoveByDirection(_direction, controller.Stat.GetValue(EEnemyValueFloat.ChargeSpeed).Value);

        float movedDistance = Vector3.Distance(_startPosition, controller.transform.position);
        if (movedDistance >= controller.Stat.GetValue(EEnemyValueFloat.ChargeDistance).Value)
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
        }
    }

    public override void Exit()
    {
        controller.Stat.DisableSuperArmor();
        controller.Anim.SetBool("IsMove", false);
        controller.Anim.SetBool("IsCharge", false);
        controller.Move.ResumeAgent();
        controller.EnablePhysics(true);
        _attack.EndCharge();
        Debug.Log("[DashState] Exit → Chase");
    }
}
