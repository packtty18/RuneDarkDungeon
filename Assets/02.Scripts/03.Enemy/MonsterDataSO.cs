using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Enemy/Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Identity")]
    public EnemyType enemyType;

    [LabelText("Monster Name")]
    public string monsterName;

    [FoldoutGroup("Base Stats")]
    [MinValue(1)]
    public int maxHP;

    [FoldoutGroup("Base Stats")]
    public int attack;

    [FoldoutGroup("Base Stats")]
    public int defense;

    [FoldoutGroup("Base Stats")]
    public float moveSpeed;


    [FoldoutGroup("Combat")]
    public float attackRange;

    [FoldoutGroup("Combat")]
    public float attackCooldown;

    [FoldoutGroup("Combat")]
    public bool hasSuperArmor;

    [ShowIf(nameof(IsBoss))]
    [FoldoutGroup("Boss")]
    public PhaseDataSO[] phases;

    private bool IsBoss()
    {
        return enemyType == EnemyType.Boss;
    }
}
