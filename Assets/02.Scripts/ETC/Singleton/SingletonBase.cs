using UnityEngine;

// 이 클래스를 직접 사용하지 말고, GlobalSingleton 또는 LocalSingleton을 상속하여 사용하십시오.
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;
    public static T Instance => s_instance;

    protected virtual bool ShouldDestroyOnLoad => false;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this as T;
        OnInit();

        //해당 싱글톤이 글로벌이면서 루트 오브젝트일때만 DontDestroyOnLoad 호출
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
            s_instance = null;
        }
    }
}