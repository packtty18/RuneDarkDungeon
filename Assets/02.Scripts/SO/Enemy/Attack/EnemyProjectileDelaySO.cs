using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyProjectileDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject _spawnPrefab;

    public override void Execute(Transform spawnPos, float damage)
    {
        GameObject obj = Instantiate(_spawnPrefab,spawnPos.position, spawnPos.rotation);

        if(obj.TryGetComponent(out EnemyProjectile proj))
        {
            proj.Init(damage);
            return;
        }

        if (obj.TryGetComponent(out EnemyExplosion explosion))
        {
            explosion.Init(damage);
            return;
        }

        Util.ObjectDestroy(obj);

    }
}

