using UnityEngine;

public class EnemySpawner : PoolSpawner
{
    [Header("Spawn Offset")]
    [SerializeField] private bool useRandomOffset = true;

    [SerializeField]
    private Vector3 fixedOffset = Vector3.zero;

    [SerializeField]
    private Vector2 randomOffsetRange = new Vector2(1.5f, 1.5f);

    public EnemyController SpawnEnemy(EPoolType poolType)
    {
        GameObject obj = GetFromPool(poolType);

        if (!obj.TryGetComponent(out EnemyController enemy))
        {
            Util.ObjectDestroy(obj);
            Debug.LogWarning("[EnemySpawner] Spawned object is not EnemyController");
            return null;
        }

        Vector3 spawnPos = CalculateSpawnPosition();
        enemy.transform.position = spawnPos;
        Debug.Log($"[SpawnManager] 스폰 위치 설정 {spawnPos}");
        return enemy;
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 basePos = transform.position;

        if (!useRandomOffset)
            return basePos + fixedOffset;

        Vector3 randomOffset = new Vector3(
            Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
            0f,
            Random.Range(-randomOffsetRange.y, randomOffsetRange.y)
        );

        return basePos + randomOffset;
    }
}
