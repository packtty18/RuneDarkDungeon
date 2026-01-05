using System;
using UnityEngine;

[Serializable]
public class PoolConfig<T> : PoolConfigBase where T : PoolableObject
{
    [SerializeField] private string _poolKey;
    [SerializeField] private T _component;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 100;
    [SerializeField] private bool _warmUp = true;
    [SerializeField] private bool _isUI = false;

    public override string PoolKey => _poolKey;

    public string GetPoolKey() => string.IsNullOrEmpty(PoolKey) ? _component.gameObject.name : PoolKey;
    public override void CreatePool(PoolManager manager)
    {
        manager.CreatePool<T>(GetPoolKey(), _component, _defaultCapacity, _maxSize, _warmUp, _isUI);
    }
}
