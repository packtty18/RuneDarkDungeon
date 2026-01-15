using UnityEngine;

public class BossAttack : EliteAttack
{
    public override void Init()
    {
        base.Init();


        RegisterStrategy(0, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //강타
        RegisterStrategy(1, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //3연베기

        //페이즈2
        //RegisterStrategy(2, new EnemySpawnAttack()  //마탄

        //페이즈3
        //RegisterStrategy(3, new EnemySpawnAttack()  //검기폭발
        //RegisterStrategy(4, new EnemySpawnAttack()  //낙뢰
    }
}
