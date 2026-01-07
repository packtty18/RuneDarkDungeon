using UnityEngine;

public class EnemyArrow : EnemyProjectile
{
    [SerializeField] HitBox _hitbox;

    public override void Init(int damage)
    {
        base.Init(damage);
        _hitbox?.Activate();
    }

    protected override void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
