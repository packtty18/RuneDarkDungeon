using UnityEngine;

public class ProtoMagicAttack : IAttackStrategy
{
    private readonly GameObject _enemyProjectile;
    private readonly Transform _spawnPos;
    public ProtoMagicAttack(GameObject magic, Transform spawnPos )
    {
        _enemyProjectile = magic;
        _spawnPos = spawnPos;
    }

    public void BeginAttack()
    {
        //마법 캐스팅
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
