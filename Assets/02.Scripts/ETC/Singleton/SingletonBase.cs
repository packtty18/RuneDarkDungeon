using UnityEngine;

//PSH : 직접 사용하지 말것. GlobalSingleton 혹은 LocalSingleton을 사용.
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance => _instance;

    protected virtual bool ShouldDestroyOnLoad => false;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
        OnInit();

        //해당 싱글톤이 글로벌이면서 루트 오브젝트일때만 DontDestoryOnLoad 호출
        if (ShouldDestroyOnLoad && transform.root == transform)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnInit()
    { 
    
    }

    //해당 싱글톤이 존재하는지 여부 반환
    public static bool IsExist()
    {
        return Instance != null;
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            _instance = null;
        }
    }
}