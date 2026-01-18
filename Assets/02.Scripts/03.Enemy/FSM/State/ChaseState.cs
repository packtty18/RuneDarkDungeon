using Unity.VisualScripting;

public class ChaseState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Chase;

    public ChaseState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();
        controller.Move.ResumeAgent();
        controller.Move.SetAbleToRatate(true);
        controller.Anim.SetBool(EnemyAnimator.s_moveBool, true);
        controller.SetConstraintsPosition(false);
    }

    public override void Tick(float deltaTime)
    {
        StateTransition transition = controller.Behavior.UpdateChase();

        if (transition.ShouldTransition)
        {
            controller.FSM.ChangeState(transition.NextState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.SetConstraintsPosition(true);
        controller.Move.SetAbleToRatate(false);
        controller.Move.PauseAgent();
        controller.Anim.SetBool(EnemyAnimator.s_moveBool, false);
    }
}