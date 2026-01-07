using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected int damage;

    public virtual void Init(int damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        Move();
    }

    protected abstract void Move();

    protected virtual void OnTriggerEnter(Collider other)
    {
        //조건처리

        Util.ObjectDestroy(gameObject);
    }
}
