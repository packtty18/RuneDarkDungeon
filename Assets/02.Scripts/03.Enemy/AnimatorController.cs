using Sirenix.OdinInspector;
using UnityEngine;

//상속할수도
public class AnimatorController : MonoBehaviour
{
    public static string s_trigger_Attack = "Attack";
    public static string s_int_AttackID = "AttackID";
    public static string s_trigger_Hit = "Hit";
    public static string s_trigger_Dead = "Dead";
    public static string s_bool_IsMove = "IsMove";
    public static string s_trigger_Reset = "Reset";

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
