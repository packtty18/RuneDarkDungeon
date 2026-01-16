using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/Buff")]
public class BuffEffectSO : AttackEffectSO
{
    [SerializeField] private BuffSO buff;

    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {

        //수정 예정
        if (owner.Owner.Buff != null)
        {
            owner.Owner.Buff.ApplyBuff(buff);
        }
    }
}
