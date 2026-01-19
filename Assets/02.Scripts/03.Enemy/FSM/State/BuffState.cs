using UnityEngine;

public class BuffState : EnemyState
{
    public override EEnemyState StateType => EEnemyState.Buff;

    private float cooldownTimer;
    public BuffState(EnemyController controller) : base(controller) { }


    public override void Enter()
    {
        base.Enter();
        controller.Attack.StartBuff();
        BossAttack attack = controller.Attack as BossAttack;
        //버프 오브젝트 생성 => 닿는 적들은 버프
        attack.InstantBuff();
        controller.Anim.SetTrigger(EnemyAnimator.s_buffTrigger);
        
        controller.Sound?.PlayBuff();
        
        cooldownTimer = 3f;
    }

    public override void Tick(float deltaTime)
    {
        cooldownTimer -= deltaTime;
        if (cooldownTimer > 0f)
        {
            return;
        }

        controller.FSM.ChangeState(EEnemyState.Idle);
    }

    public override void Exit()
    {
        controller.Attack.EndBuff();
        base.Exit();

    }
}
