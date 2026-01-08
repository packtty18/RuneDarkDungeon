using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private readonly int _comboIndexHash = Animator.StringToHash("ComboIndex");
    private readonly int _attackTypeHash = Animator.StringToHash("AttackType");
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");

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
    
    public void SetAttackTrigger()
    {
        _animator.SetTrigger(_attackHash);
    }

    /// <summary>
    /// 공격 애니메이션 실행 (0-based index)
    /// </summary>
    public void PlayAttack(int comboIndex, EAttackType attackType)
    {
        _animator.SetInteger(_comboIndexHash, comboIndex);
        _animator.SetInteger(_attackTypeHash, (int)attackType);
        _animator.SetBool(_isAttackingHash, true);
        _animator.SetTrigger(_attackHash);
    }

    public void SetComboIndex(int index)
    {
        _animator.SetInteger(_comboIndexHash, index);
    }

    public void SetAttackType(EAttackType type)
    {
        _animator.SetInteger(_attackTypeHash, (int)type);
    }

    /// <summary>
    /// 콤보 리셋 (0으로 초기화)
    /// </summary>
    public void ResetCombo()
    {
        _animator.SetInteger(_comboIndexHash, 0);
    }

    /// <summary>
    /// 공격 종료 (Animation Event에서 호출)
    /// </summary>
    public void OnAttackAnimationEnd()
    {
        _animator.SetBool(_isAttackingHash, false);
    }
}
