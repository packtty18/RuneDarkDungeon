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

    [ShowIf(nameof(HasCharge))]
    [FoldoutGroup("Charge")]
    public float chargeSpeed;
    [ShowIf(nameof(HasCharge))]
    [FoldoutGroup("Charge")]
    public float chargeRange; //플레이어가 chargeRange안에 들경우 Charge실행
    [ShowIf(nameof(HasCharge))]
    [FoldoutGroup("Charge")]
    public float chargeDistance; //Charge의 이동 거리
    [ShowIf(nameof(HasCharge))]
    [FoldoutGroup("Charge")]
    public float chargeCooldown;
    

    private bool HasCharge()
    {
        return enemyType == EEnemyType.Elite || enemyType == EEnemyType.Boss;
    }

}
