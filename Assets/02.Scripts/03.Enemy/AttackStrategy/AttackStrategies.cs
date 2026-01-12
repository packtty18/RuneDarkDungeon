using UnityEngine;

//근접 공격 : 자체의 히트박스를 끄고 킴
public class ProtoMeleeAttack : IActionStrategy
{
    private readonly HitboxController _hitbox;
    private readonly string _key;
    private readonly float _damage;

    private float _loopDelay = 0;
    public float LoopDelay => _loopDelay;

    public ProtoMeleeAttack(HitboxController hitbox, string key, float damage)
    {
        _hitbox = hitbox;
        _key = key;
        _damage = damage;
    }

    public void BeginAction()
    {
        _hitbox.Activate(_key, _damage);
    }

    public void EndAction()
    {
        _hitbox.Deactivate(_key);
    }
}

//화살,마법탄 등을 생성. 히트박스는 생성된 객체에 존재
public class ProtoRangedAttack : IActionStrategy
{
    private readonly EnemyDelaySOBase _windup;
    private readonly Transform _spanwPos;
    private readonly float _damage;
    public float LoopDelay => _windup.Delay;

    public ProtoRangedAttack(
        EnemyDelaySOBase windup,
        Transform spawnPos,
        float damage)
    {
        _windup = windup;
        _spanwPos = spawnPos;
        _damage = damage;
    }

    public void BeginAction()
    {
        _windup.BeginLoop();
    }

    public void EndAction()
    {
        _windup.Execute(_spanwPos, _damage);
    }
}

//버프 혹은 특수
public class ProtoMagic : IActionStrategy
{
    public ProtoMagic()
    {

    }

    //버프 캐스팅 대기시간
    public float LoopDelay => throw new System.NotImplementedException();

    public void BeginAction()
    {
    }

    public void EndAction()
    {
    }
}
