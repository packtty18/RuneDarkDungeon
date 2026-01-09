using System;
using UnityEngine;

// 적의 스폰만을 다루는 스포너
public class EnemySpawner : PoolSpawner
{
    public EnemyController SpawnEnemy(EPoolType poolType)
    {
        GameObject obj = GetFromPool(poolType);

        if(!obj.TryGetComponent(out EnemyController enemy))
        {
            Util.ObjectDestroy(obj);
            Debug.Log("해당 오브젝트는 적 개체가 아님");
            return null;
        }

        return enemy;
    }
}
