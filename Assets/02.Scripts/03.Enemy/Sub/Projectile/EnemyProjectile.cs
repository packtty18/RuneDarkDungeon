using Sirenix.OdinInspector;
using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _damage;

    [Header("Life Time")]
    [SerializeField] private float lifeTime = 5f; // seconds

    private float _lifeTimer;
    private bool _onInit = false;

    [Button]
    public virtual void Init(float damage)
    {
        _damage = damage;
        _lifeTimer = lifeTime;
        _onInit = true;
        //Debug.Log($"[EnemyProjectile] Initialized (Damage: {damage}, LifeTime: {lifeTime}s)");
    }

    private void OnDisable()
    {
        _onInit = false;
    }

    private void Update()
    {
        if(!_onInit )
        {
            return;
        }   

        Move();
        UpdateLifeTime();
    }

    protected abstract void Move();

    private void UpdateLifeTime()
    {
        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer <= 0f)
        {
            //Debug.Log("[EnemyProjectile] Auto destroyed (LifeTime expired)");
            Util.ObjectDestroy(gameObject);
        }
    }

    protected abstract void OnTrigger();


    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Environment") || other.CompareTag("Player"))
        {
            OnTrigger();
        }
    }
}
