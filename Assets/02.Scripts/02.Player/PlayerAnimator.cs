using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetSpeedRatio(float ratio)
    {
        _animator.SetFloat(_speedRatioHash, ratio);
    }

    public void SetJump(bool isJumping)
    {
        _animator.SetBool(_jumpHash, isJumping);
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger(_attackHash);
    }
    
    public void ResetAttackTrigger()
    {
        _animator.SetTrigger(_attackHash);
    }

}
