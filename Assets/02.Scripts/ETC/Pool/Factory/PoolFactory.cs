using UnityEngine;

/*
 * PoolFactory<T>
 * 
 * 목적:
 * - PoolManager 기반의 풀 오브젝트 생성을 캡슐화하기 위한 베이스 팩토리 클래스.
 * - 게임 로직에서 PoolManager 직접 접근을 최소화하기 위함.
 *
 * 사용 방법:
 * 1. PoolableObject를 상속한 타입 T를 만든다.
 * 2. PoolFactory<T>를 상속한 전용 Factory를 만든다.
 * 3. 생성자에서 PoolManager와 PoolKey를 전달한다.
 * 4. 초기화는 T의 OnSpawn()에서 수행한다. 
 * 5. 생성 시 필요한 로직은 OnCreated()를 오버라이드하여 구현한다.
 *
 * 주의:
 * - Factory는 MonoBehaviour가 아님.
 * - new 키워드로 생성하여 사용.
 * - PoolManager.Instance를 내부에서 직접 참조하지 않음.
 * test/BoxFactory.cs 참고.
 */
public abstract class PoolFactory<T> where T : PoolableObject
{
    protected readonly PoolManager _poolManager;
    protected readonly string _key;

    protected PoolFactory(PoolManager poolManager, string key)
    {
        _poolManager = poolManager;
        _key = key;   
    }

    protected T CreateInternal()
    {
        var obj = _poolManager.Get<T>(_key);
        OnCreated(obj);
        return obj;
    }

    protected virtual void OnCreated(T obj) { }
}
