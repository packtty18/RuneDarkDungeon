using UnityEngine;

//임시제작 프로젝타일
public class EnemyProjectile : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected int _damage;
    [SerializeField] private HitBox _hitbox;

    private void Awake()
    {
        _hitbox = GetComponentInChildren<HitBox>();
    }

    public virtual void Init(int damage)
    {
        _hitbox.Activate();
        _damage = damage;
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
