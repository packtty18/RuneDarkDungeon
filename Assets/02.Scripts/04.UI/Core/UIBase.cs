using Sirenix.OdinInspector;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField,ReadOnly]protected bool _isInitialized;
    [SerializeField] private bool _awakeInit;
    protected virtual void Awake()
    {
        if(_awakeInit)
        {
            Init();
        }
    }

    public virtual void Init()
    {
        OnInit();
        _isInitialized = true;
    }

    protected virtual void OnInit()
    {
        // 확장 구현한 요소의 초기화 요소 작성
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
