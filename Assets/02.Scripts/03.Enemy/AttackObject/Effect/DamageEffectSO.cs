using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/Damage")]
public class DamageEffectSO : AttackEffectSO
{
    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {
        owner.ActivateHitBox(position);
        //Debug.Log("[AttackEffect] DamageEffect executed");
    }
}
