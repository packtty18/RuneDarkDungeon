using System;
using System.Collections;
using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{
    private EPoolType _poolType;
    private Coroutine _autoReleaseCoroutine;

    public event Action<EPoolType, GameObject> OnReturnRequested;

    public void SetPoolType(EPoolType poolType)
    {
        _poolType = poolType;
    }

    public virtual void OnSpawn()
    {
        // PSH : 현재 이코드에 의해 스폰시 풀Root 기준의 원점으로 이동하는 문제가 발생합니다.
        // 위치 초기화는 하위 클래스에서 처리하도록 변경이 필요합니다.
        // transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public virtual void OnDespawn()
    {
       
    }

    public void ReturnToPool()
    {
        if (_poolType.Equals(EPoolType.None))
        {
            Debug.LogWarning($"[PoolableObject] {gameObject.name}의 풀 키가 없습니다. 오브젝트를 파괴합니다.");
            Destroy(gameObject);
            return;
        }

        OnReturnRequested?.Invoke(_poolType, this.gameObject);
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
