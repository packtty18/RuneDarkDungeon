using UnityEngine;

public class ProtoMeleeAttack : IAttackStrategy
{
    private readonly HitboxController _hitbox;
    public ProtoMeleeAttack( HitboxController hitbox)
    {
        _hitbox = hitbox;
    }

    public void BeginAttack()
    {
        _hitbox.Active("Main");
    }

    public void OnLoopEnd()
    {
        //근접공격은 필요없음
    }

    public void EndAttack()
    {
        _hitbox.DeActive("Main");
    }
}

