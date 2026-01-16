using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyAttack/Spawn")]
public class SpawnEffectSO : AttackEffectSO
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnCount = 1;
    [SerializeField] private float spawnDelay = 0f;

    public override void Execute(EnemyAttackObject owner, Vector3 position)
    {
        owner.StartSpawnRoutine(prefab, position, spawnCount, spawnDelay);
    }
}
