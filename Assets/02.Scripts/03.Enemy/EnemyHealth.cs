using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public ETeamType Team => ETeamType.Enemy;

    public void ApplyDamage(DamageData data)
    {
        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }
}
