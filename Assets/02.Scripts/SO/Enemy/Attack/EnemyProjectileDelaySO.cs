using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyProjectileDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject _spawnPrefab;

    public override void Execute(EnemyController owner, Transform spawnPos, float damage)
    {
        GameObject obj = Instantiate(_spawnPrefab,spawnPos.position, spawnPos.rotation);
        if (obj.TryGetComponent(out EnemyAttackObject attack))
        {
            attack.Initialize(owner , damage);
            return;
        }

        Util.ObjectDestroy(obj);

    }
}

