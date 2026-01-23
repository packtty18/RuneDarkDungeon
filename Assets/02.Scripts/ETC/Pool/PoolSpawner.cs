using UnityEngine;

//풀을 통해 오브젝트를 가져오는 역할을 함.
public class PoolSpawner : MonoBehaviour
{
    protected GameObject GetFromPool(EPoolType poolType)
    {
        GameObject obj = PoolManager.Instance.Get(poolType);
        if (obj == null)
        {
            return null;
        }

        return obj;
    }
}
