using UnityEngine;

public class ArchorAttack : EnemyAttack
{
    [SerializeField] private EnemyDelaySOBase _arrow;
    [SerializeField] private Transform _arrowSpawnPos;
    public override void Init()
    {
        base.Init();
        RegisterStrategy(0, new EnemySpawnAttack(_arrow, _arrowSpawnPos, _damage));   //기본공격1
    }
}
