using Sirenix.OdinInspector;
using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/Spawn")]
public class SpawnEffectSO : AttackEffectSO
{
    [SerializeField] private bool isPoolable;
    [SerializeField, ShowIf(nameof(isPoolable))] private EPoolType poolType;
    [SerializeField, HideIf(nameof(isPoolable))] private GameObject prefab;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private float spawnDelay = 0f;

    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {
        if(isPoolable && PoolManager.IsExist())
        {
            owner.StartSpawnRoutine(poolType, position, spawnCount, spawnDelay);
        }
        else
        {
            owner.StartSpawnRoutine(prefab, position, spawnCount, spawnDelay);
        }
        
    }
}
