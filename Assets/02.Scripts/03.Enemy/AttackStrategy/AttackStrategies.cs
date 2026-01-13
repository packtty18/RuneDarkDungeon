using UnityEngine;

//소유한 히트박스를 활성화하여 공격
public class EnemyDirectAttack : IAttackStretagy
{
    private readonly HitboxController _hitbox;
    private readonly string _key;
    private readonly float _damage;

    private float _loopDelay = 0;
    public float LoopDelay => _loopDelay;

    public EnemyDirectAttack(HitboxController hitbox, string key, float damage)
    {
        _hitbox = hitbox;
        _key = key;
        _damage = damage;
    }

    public void BeginAttack()
    {
        _hitbox.Activate(_key, _damage);
    }

    public void EndAttack()
    {
        _hitbox.Deactivate(_key);
    }
}

//히트박스를 가진 다른 객체를 생성하여 공격
// Hitbox spawn attack strategy
public class EnemySpawnAttack : IAttackStretagy
{
    private readonly EnemyDelaySOBase _delayLoop;
    private readonly Transform _spawnPos;
    private readonly float _damage;

    private bool _executed;

    public float LoopDelay => _delayLoop.Delay;

    public EnemySpawnAttack(EnemyDelaySOBase delayLoop, Transform spawnPos, float damage)
    {
        _delayLoop = delayLoop;
        _spawnPos = spawnPos;
        _damage = damage;
    }

    public void BeginAttack()
    {
        _executed = false;
        _delayLoop.BeginLoop();
    }

    public void EndAttack()
    {
        if (_executed)
            return;

        _executed = true;
        _delayLoop.Execute(_spawnPos, _damage);
    }
}
