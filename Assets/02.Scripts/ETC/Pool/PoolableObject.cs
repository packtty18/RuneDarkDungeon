using System;
using System.Collections;
using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{
    private string _poolKey;
    private Coroutine _autoReleaseCoroutine;

    public event Action<string, PoolableObject> OnReturnRequested;

    public void SetPoolKey(string poolKey)
    {
        _poolKey = poolKey;
    }

    public virtual void OnSpawn()
    {

    }

    public virtual void OnDespawn()
    {
       
    }

    public void ReturnToPool()
    {
        if (string.IsNullOrEmpty(_poolKey))
        {
            Debug.LogWarning($"[PoolableObject] {gameObject.name}의 풀 키가 없습니다. 오브젝트를 파괴합니다.");
            Destroy(gameObject);
            return;
        }

        OnReturnRequested?.Invoke(_poolKey, this);
    }

    public void ReturnToPoolAfter (float delay)
    {
        if (_autoReleaseCoroutine != null)
        {
            StopCoroutine(_autoReleaseCoroutine);
        }
        _autoReleaseCoroutine = StartCoroutine(AutoReturnCoroutine(delay));
    }

    private IEnumerator AutoReturnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    protected virtual void OnDestroy()
    {
        if (_autoReleaseCoroutine != null)
        {
            StopCoroutine(_autoReleaseCoroutine);
            _autoReleaseCoroutine = null;
        }
    }

}
