using UnityEngine;

public class ProtoArrowAttack : IAttackStrategy
{
    private readonly Transform _spawnPos;
    private readonly GameObject _enemyProjectile;

    public ProtoArrowAttack(GameObject projectile,Transform spawnPos)
    {
        _spawnPos = spawnPos;
        _enemyProjectile = projectile;
    }
    public void BeginAttack()
    {
        //조준 시작
    }
    public void OnLoopEnd()
    {
        //발사체 생성
    }
    public void EndAttack()
    { 
        //완전한 공격의 종료
    }
}
