using System.Threading.Tasks;

public class IdleState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Idle;

    public IdleState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Tick(float deltaTime)
    {
        if (controller.Wait)
            return;
        if (controller.IsTargetExist())
        {
            controller.FSM.ChangeState(EEnemyState.Chase);
        }
    }
}