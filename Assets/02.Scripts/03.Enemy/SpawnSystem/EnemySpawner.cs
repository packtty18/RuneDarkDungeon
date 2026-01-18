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

        // 위치 설정을 먼저 수행 (Agent가 비활성화된 상태)
        Vector3 spawnPos = CalculateSpawnPosition();
        enemy.transform.position = spawnPos;
        
        Debug.Log($"[EnemySpawner] 스폰 위치 설정: {spawnPos}, 실제 위치: {enemy.transform.position}");
        
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
