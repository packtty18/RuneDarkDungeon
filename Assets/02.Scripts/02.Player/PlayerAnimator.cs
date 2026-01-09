using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMove))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private PlayerMove _playerMove;

    private readonly int _speedRatioHash = Animator.StringToHash("Blend");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _attackHash = Animator.StringToHash("Attack");
    private readonly int _canMoveHash = Animator.StringToHash("CanMove");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        _playerMove.OnCanMoveChanged += SetCanMove;
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

    public void PlayComboAttack(int comboIndex, EAttackType attackType)
    {
        string stateName = $"{attackType.ToString()}_Combo{comboIndex}";

        _animator.CrossFade(stateName, 0.05f, 1, 0);
        _animator.CrossFade(stateName, 0.05f, 2, 0);
    }

    public void PlayChargeFinisher(EAttackType attackType)
    {
        string stateName = $"AttackSubStateMachine.{attackType.ToString()}_ChargeFinisher";

        _animator.CrossFade(stateName, 0.05f, 0, 0);
    }

    public void PlaySkill(EAttackType attackType)
    {
        string stateName = $"AttackSubStateMachine.{attackType.ToString()}";

        _animator.CrossFade(stateName, 0.1f, 0, 0);
    }

    public void SetCanMove(bool canMove)
    {
        _animator.SetBool(_canMoveHash, canMove);
    }

    private void OnDestroy()
    {
        _playerMove.OnCanMoveChanged -= SetCanMove;
    }
}
