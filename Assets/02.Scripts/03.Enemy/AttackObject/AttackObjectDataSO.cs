using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AttackObjectData",menuName = "SO/EnemyAttack/Attack Object Data")]
public class AttackObjectDataSO : ScriptableObject
{
    [Header("Life")]
    public float LifeTime = 5f;

    [Header("Effects")]
    public List<AttackEffectSO> Effects;
}
