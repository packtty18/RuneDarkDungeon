using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerStateMachine _stateMachine;
    private AnimatorOverrideController overrideController;

    [SerializeField] private AnimationClip defaultSkillClip;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private readonly int _canMoveHash = Animator.StringToHash("CanMove");
    private readonly int _dodgeHash = Animator.StringToHash("Dodge");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = overrideController;
    }

    private void Start()
    {
        _stateMachine.OnCanMoveChanged += SetCanMove;
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

    public void SetDodge(bool isDodging)
    {
        _animator.SetBool(_dodgeHash, isDodging);
    }


    public void PlayComboAttack(int comboIndex)
    {
        string stateName = $"Basic_Combo{comboIndex}";

        _animator.CrossFade(stateName, 0.05f, 1, 0);
        _animator.CrossFade(stateName, 0.05f, 2, 0);
    }

    public void PlayChargeFinisher()
    {
        string stateName = $"AttackSubStateMachine.Basic_ChargeFinisher";

        _animator.CrossFade(stateName, 0.05f, 0, 0);
    }

    public void PlaySingleAttack(EAttackType attackType)
    {
        string stateName = $"AttackSubStateMachine.{attackType.ToString()}";

        _animator.CrossFade(stateName, 0.1f, 0, 0);
    }

    public void PlaySkill(AnimationClip clip)
    {
        string stateName = $"AttackSubStateMachine.Skill";
        overrideController[defaultSkillClip] = clip;

        _animator.CrossFade(stateName, 0.1f, 0, 0);
    }

    public void SetCanMove(bool canMove)
    {
        _animator.SetBool(_canMoveHash, canMove);
    }

    private void OnDestroy()
    {
        _stateMachine.OnCanMoveChanged -= SetCanMove;
    }
}
