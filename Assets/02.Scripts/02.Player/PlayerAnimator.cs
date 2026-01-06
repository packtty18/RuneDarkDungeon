using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSpeedRatio(float ratio)
    {
        animator.SetFloat(_speedRatioHash, ratio);
    }

    public void SetJump(bool isJumping)
    {
        animator.SetBool(_jumpHash, isJumping);
    }

    public void SetAttack(bool isAttacking)
    {
        animator.SetBool(_attackHash, isAttacking);
    }

}
