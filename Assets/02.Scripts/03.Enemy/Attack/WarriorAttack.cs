using UnityEngine;

public class WarriorAttack : EnemyAttack
{
    public override void Init()
    {
        base.Init();
        RegisterStrategy(0, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //기본공격1
        RegisterStrategy(1, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //기본공격2
    }
}
