using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/Delay/Projectile")]
public class EnemyAimDelaySO : EnemyDelaySOBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _damage;

    public override void Execute(Transform spawnPos)
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
        proj.Init(_damage);

        Debug.Log("[Windup] Arrow Fired");
    }
}

