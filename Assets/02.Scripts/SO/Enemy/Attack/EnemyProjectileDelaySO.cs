using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyProjectileDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private bool isPoolable;
    [SerializeField, ShowIf(nameof(isPoolable))] private EPoolType poolType;
    [SerializeField, HideIf(nameof(isPoolable))] private GameObject prefab;

    public override void Execute(EnemyController owner, Transform spawnPos, float damage)
    {
        GameObject obj;
        if (isPoolable && PoolManager.IsExist())
        {
            obj = PoolManager.Instance.Get(poolType);
        }
        else
        {
            obj = Instantiate(prefab);
        }
        obj.transform.position = spawnPos.position;
        obj.transform.rotation = spawnPos.rotation;
        if (obj.TryGetComponent(out EnemyAttackObject attack))
        {
            attack.Initialize(owner , damage);
            return;
        }

        Util.ObjectDestroy(obj);

    }
}

