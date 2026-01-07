using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]private ETeamType _team;
    public ETeamType Team => _team;

    public void ApplyDamage(DamageData data)
    {
        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }
}
