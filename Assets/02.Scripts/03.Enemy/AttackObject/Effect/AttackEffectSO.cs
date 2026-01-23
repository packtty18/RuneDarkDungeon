using UnityEngine;

public abstract class AttackEffectSO : ScriptableObject
{
    public abstract void Execute(EnemyAttackObject owner,Vector3 position);
}
