using UnityEngine;

//원거리 공격의 베이스
public class RangedAttack : AttackBase
{
    public override void Execute()
    {
        base.Execute();
        Debug.Log("[RangedAttack] 공격 실행", this);
    }

    public override void Cancel()
    {
        base.Cancel();
        Debug.Log("[RangedAttack] 공격 취소", this);
    }
}
