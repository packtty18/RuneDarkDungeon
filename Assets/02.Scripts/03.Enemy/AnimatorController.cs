using Sirenix.OdinInspector;
using UnityEngine;

//상속할수도
public class AnimatorController : MonoBehaviour
{
    public static string s_triggerAttack = "Attack";
    public static string s_intAttackID = "AttackID";
    public static string s_triggerHit = "Hit";
    public static string s_triggerDead = "Dead";
    public static string s_boolIsMove = "IsMove";
    public static string s_triggerReset = "Reset";

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
