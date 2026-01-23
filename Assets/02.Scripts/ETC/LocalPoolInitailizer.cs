using System.Collections.Generic;
using UnityEngine;

public class LocalPoolInitailizer : MonoBehaviour
{
    [SerializeField] private List<PoolConfigSO> _poolConfigs = new List<PoolConfigSO>();

    public void InitPool()
    {
        PoolManager.Instance.CreatePoolsFromConfigs(_poolConfigs);
    }
}
