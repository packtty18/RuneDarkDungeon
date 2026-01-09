using Sirenix.OdinInspector;
using UnityEngine;

//상속할수도
public class AnimatorController : MonoBehaviour
{
    public static string s_attackTrigger = "Attack";
    public static string s_attackIdInt = "AttackID";
    public static string s_hitTrigger = "Hit";
    public static string s_deadTrigger = "Dead";
    public static string s_moveBool = "IsMove";
    public static string s_resetTrigger = "Reset";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public virtual void Init()
    {
        Debug.Log("AniamtorController 초기화", this);
    }

    [Button]
    public void SetInt(string name, int value)
    {
        _animator.SetInteger(name, value);
    }
    [Button]
    public void SetFloat(string name, float value)
    {
        _animator.SetFloat(name, value);
    }
    [Button]
    public void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }
    [Button]
    public void SetBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }

    public void SetRootMotion(bool enable)
    {
        _animator.applyRootMotion = enable;
    }

}
