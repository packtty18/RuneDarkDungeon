using NUnit.Framework;
using UnityEngine;

public class BossAttack : EliteAttack
{
    [SerializeField] private EnemyDelaySOBase _magicMissileData;
    [SerializeField] private EnemyDelaySOBase _bloodExplosionData;
    [SerializeField] private EnemyDelaySOBase _thunderStormData;

    [SerializeField] private Transform _bulletSpawnPos;
    [SerializeField] private EnemySpawnManager _allySpawnManager;  //보스를 스폰한 매니저

    public EnemySpawnManager TargetSpawner => _allySpawnManager;
    public override void Init()
    {
        base.Init();

        RegisterStrategy(0, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //강타
        RegisterStrategy(1, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //3연베기
    }


    public void Phase2Strategy()
    {
        //페이즈2
        RegisterStrategy(2, new EnemySpawnAttack(_magicMissileData, _bulletSpawnPos, _damage)); //3연 마탄
    }

    public void Phase3Strategy()
    {
        //페이즈3
        RegisterStrategy(3, new EnemySpawnAttack(_bloodExplosionData, transform, _damage)); //검기폭발
        RegisterStrategy(4, new EnemySpawnAttack(_thunderStormData, _bulletSpawnPos, _damage)); //낙뢰
    }
}
