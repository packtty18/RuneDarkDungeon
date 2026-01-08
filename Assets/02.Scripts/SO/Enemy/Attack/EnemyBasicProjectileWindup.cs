using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyAimDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int damage;

    public override void Execute(Transform spawnPos)
    {
        GameObject arrow = Instantiate(
            targetPrefab,
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

