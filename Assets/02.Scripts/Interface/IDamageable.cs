using UnityEngine;

public interface IDamageable 
{
    ETeamType Team { get; }
    void ApplyDamage(DamageData data);
}
