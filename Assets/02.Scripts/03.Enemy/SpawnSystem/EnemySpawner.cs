using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController Spawn(EPoolType poolType)
    {
        GameObject obj = PoolManager.Instance.Get(poolType);
        if (obj == null)
        {
            Debug.LogError($"[EnemySpawner] Spawn failed : {poolType}");
            return null;
        }

        obj.transform.position = transform.position;
        return obj.GetComponent<EnemyController>();
    }
}
