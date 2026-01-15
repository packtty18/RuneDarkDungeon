
public class SummonState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Summon;
    public SummonState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Attack.StartSummon();

        
    }

    public override void Tick(float deltaTime)
    {

    }

    public override void Exit()
    {
        controller.Attack.EndSummon();
        base.Exit();

    }
}
