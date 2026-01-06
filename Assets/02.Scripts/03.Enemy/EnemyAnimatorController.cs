using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    public const string PARAMETER_ATTACK = "Attack";
    public const string PARAMETER_ATTACKID = "AttackID";
    public const string PARAMETER_HIT = "Hit";
    public const string PARAMETER_DEAD = "Dead";
    public const string PARAMETER_MOVE = "IsMove";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
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

    [Button]
    private void SetRootToAnim()
    {
        if (_animator == null)
        {
            return;
        }

        transform.position += _animator.deltaPosition;
        transform.rotation *= _animator.deltaRotation;
    }
}
