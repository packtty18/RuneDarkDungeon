using UnityEngine;

public class WarriorAttack : EnemyAttack
{
    [SerializeField] protected HitboxController _hitboxController;

    public override void Init()
    {
        base.Init();
        _hitboxController = GetComponentInChildren<HitboxController>();
        RegisterStrategy(0, new ProtoMeleeAttack(_hitboxController, "Main", _damage));   //기본공격1
        RegisterStrategy(1, new ProtoMeleeAttack(_hitboxController, "Main", _damage));   //기본공격2
    }
}
