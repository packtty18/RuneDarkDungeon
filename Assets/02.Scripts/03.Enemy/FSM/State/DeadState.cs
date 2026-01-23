
using UnityEngine;

public class DeadState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Dead;

    public DeadState(EnemyController controller) : base(controller) { }

    public override void Enter()
    {
        base.Enter();

        controller.Shader.ResetAll();
        controller.CancelAttack();
        controller.Move.PauseAgent();
        controller.Move.SetAbleToRatate(false);

        if (controller.Stat.EnemyType == EEnemyType.Boss)
        {
            BossAttack attack = controller.Attack as BossAttack;
            attack.TargetSpawner.KillAll();
        }
        
        controller.Anim.SetTrigger(EnemyAnimator.s_deadTrigger);
        controller.Sound?.PlayDeath();
        controller.DropItem();
        controller.Dead();
        Debug.Log("사망");
    }

    public override void Tick(float deltaTime)
    {
    }
}