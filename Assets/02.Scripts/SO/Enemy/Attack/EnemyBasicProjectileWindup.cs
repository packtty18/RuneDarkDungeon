using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyAimDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject _targetPrefab;

    public override void Execute(Transform spawnPos, float damage)
    {
        GameObject arrow = Instantiate(
            _targetPrefab,
            spawnPos.position,
            spawnPos.rotation);

        if(!arrow.TryGetComponent(out EnemyProjectile proj))
        {
            Util.ObjectDestroy(arrow);
            return;
        }
        proj.Init(damage);

        Debug.Log("[Windup] Arrow Fired");
    }
}

