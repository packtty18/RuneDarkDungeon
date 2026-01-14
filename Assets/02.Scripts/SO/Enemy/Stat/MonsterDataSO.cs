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

    [ShowIf(nameof(IsElite))]
    [FoldoutGroup("Elite")]
    public float chargeSpeed;
    [ShowIf(nameof(IsElite))]
    [FoldoutGroup("Elite")]
    public float chargeRange; //플레이어가 chargeRange안에 들경우 Charge실행
    [FoldoutGroup("Elite")]
    public float chargeDistance; //Charge의 이동 거리
    [ShowIf(nameof(IsElite))]
    [FoldoutGroup("Elite")]
    public float chargeCooldown;
    
    [ShowIf(nameof(IsBoss))]
    [FoldoutGroup("Boss")]
    public PhaseDataSO[] phases;

    private bool IsBoss()
    {
        return enemyType == EEnemyType.Boss;
    }

    private bool IsElite()
    {
        return enemyType == EEnemyType.Elite;
    }

}
