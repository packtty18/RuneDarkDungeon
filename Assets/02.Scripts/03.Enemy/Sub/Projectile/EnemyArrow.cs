using UnityEngine;

public class EnemyArrow : EnemyProjectile
{
    [SerializeField] private HitBox _hitbox;

    public override void Init(float damage)
    {
        base.Init(damage);
        _hitbox = GetComponentInChildren<HitBox>();
        _hitbox.Activate(damage);
    }

    protected override void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    protected override void OnTrigger()
    {
        Util.ObjectDestroy(gameObject);
        _hitbox.Deactivate();
    }
}
