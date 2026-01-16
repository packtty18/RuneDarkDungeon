using UnityEngine;

public class MageAttack : EnemyAttack
{
    [SerializeField] private EnemyDelaySOBase _magicShot;
    [SerializeField] private Transform _magicSpawnPos;
    public override void Init()
    {
        base.Init();
        RegisterStrategy(0, new EnemySpawnAttack(_magicShot,controller, _magicSpawnPos, _damage));   //기본공격1
    }
}
