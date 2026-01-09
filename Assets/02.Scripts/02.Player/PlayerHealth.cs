using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public ETeamType Team => ETeamType.Player;

    private PlayerStats _stats;
    public void ApplyDamage(DamageData data)
    {
        _stats.Health.Consume(data.Damage);

        if (_stats.Health.IsEmpty())
        {
            //사망
        }
        else
        {
            //경직
        }


        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    private void Start()
    {
        _stats = GetComponent<PlayerStats>();
    }
}
