using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolConfig", menuName = "SO/Pool Configuration")]
public class PoolConfigSO : ScriptableObject
{
    [SerializeField] private EPoolType _poolType;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 100;
    [SerializeField] private bool _warmUp = true;
    [SerializeField] private bool _isUI = false;

    public EPoolType PoolType => _poolType;

    public void CreatePool(PoolManager manager)
    {
        manager.CreatePool(_poolType, _prefab, _defaultCapacity, _maxSize, _warmUp, _isUI);
    }
}
