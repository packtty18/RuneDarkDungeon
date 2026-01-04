using System.Collections;
using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{
    private string _poolKey;
    private Coroutine _autoReleaseCoroutine;

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

    /// <summary>
    ///  풀 수동 반환
    /// </summary>
    public void ReturnToPool()
    {
        if (string.IsNullOrEmpty(_poolKey))
        {
            Debug.LogWarning($"[PoolableObject] {gameObject.name}의 풀 키가 없습니다. 오브젝트를 파괴합니다.");
            Destroy(gameObject);
            return;
        }

        PoolManager.Instance.ReleaseByKey(_poolKey, this);
    }

    /// <summary>
    /// 일정 시간 후 풀 반환
    /// </summary>
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
