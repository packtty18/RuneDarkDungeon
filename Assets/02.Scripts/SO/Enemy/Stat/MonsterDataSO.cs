using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SO/Enemy/Monster Data")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Identity")]
    public EEnemyType enemyType;

    [LabelText("Monster Name")]
    public string monsterName;

    [FoldoutGroup("Base Stats")]
    [MinValue(1)]
    public int maxHP;

    [FoldoutGroup("Base Stats")]
    public float attack;

    [FoldoutGroup("Base Stats")]
    public float defense;

    [FoldoutGroup("Base Stats")]
    public float moveSpeed;

    [FoldoutGroup("Combat")]
    public float attackRange;

    [FoldoutGroup("Combat")]
    public float attackCooldown; //공격 후 다음 공격까지의 딜레이임.

    [FoldoutGroup("Combat")]
    public bool hasSuperArmor;

    [ShowIf(nameof(IsArchor))]
    [FoldoutGroup("Archor")]
    public float retreatRange;  //후퇴 범위. 해당 범위 밖으로 이동


    [ShowIf(nameof(IsBoss))]
    [FoldoutGroup("Boss")]
    public PhaseDataSO[] phases;

    private bool IsBoss()
    {
        return enemyType == EEnemyType.Boss;
    }

    private bool IsArchor()
    {
        return enemyType == EEnemyType.Archer;
    }

    private bool IsMage()
    {
        return enemyType == EEnemyType.Mage;
    }
}
