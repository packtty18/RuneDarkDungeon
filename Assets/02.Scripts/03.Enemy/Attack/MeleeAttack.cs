using UnityEngine;

//근접 공격 베이스
public class MeleeAttack : AttackBase
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("[MeleeAttack] 공격 실행", this);
    }

    public override void Cancel()
    {
        base.Cancel();
        Debug.Log("[MeleeAttack] 공격 취소", this);
    }
}
