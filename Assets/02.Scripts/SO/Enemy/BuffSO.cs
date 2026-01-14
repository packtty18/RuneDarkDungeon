using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Buff")]
public class BuffSO : ScriptableObject
{
    public bool HasDuration;
    [ShowIf(nameof(HasDuration))]
    public float Duration;

    public EEnemyValueFloat TargetStat;
    public float Value;
    public EStatModType ModType;
}
