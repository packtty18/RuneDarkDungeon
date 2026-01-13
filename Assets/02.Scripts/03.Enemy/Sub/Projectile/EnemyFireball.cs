using Unity.VisualScripting;
using UnityEngine;

public class EnemyFireball : EnemyProjectile
{
    [SerializeField]private GameObject _explosionPrefab;
    protected override void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    protected override void OnTrigger()
    {
        GameObject obj = Instantiate(_explosionPrefab);
        obj.transform.position = transform.position;

        if (obj.TryGetComponent(out EnemyExplosion explosion))
        {
            explosion.Init(_damage, 3);
        }

        Util.ObjectDestroy(gameObject);
    }
}
