using UnityEngine;

//근접 공격 : 자체의 히트박스를 끄고 킴
public class ProtoMeleeAttack : IActionStrategy
{
    private readonly HitboxController _hitbox;
    private float _loopDelay = 0;
    public float LoopDelay => _loopDelay;

    public ProtoMeleeAttack(HitboxController hitbox)
    {
        _hitbox = hitbox;
    }

    public void BeginAction()
    {
        _hitbox.Active("Main");
    }

    public void EndAction()
    {
        _hitbox.DeActive("Main");
    }
}

//화살,마법탄 등을 생성. 히트박스는 생성된 객체에 존재
public class ProtoRangedAttack : IActionStrategy
{
    private readonly EnemyWindup _windup;
    private readonly Transform _spanwPos;
    public float LoopDelay => _windup.Delay;

    public ProtoRangedAttack(
        EnemyWindup windup,
        Transform spawnPos)
    {
        _windup = windup;
        _spanwPos = spawnPos;
    }

    public void BeginAction()
    {
        _windup.BeginWindup();
    }

    public void EndAction()
    {
        _windup.Execute(_spanwPos);
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
