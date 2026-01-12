using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;
    
    private EMovementState _moveState;

    private bool _isJumping;

    private Coroutine _comboTimerCoroutine;

    [SerializeField] 
    private Renderer[] _playerRenderers;
    [SerializeField]
    private ParticleSystem _dashVFX;
    [SerializeField] 
    private PlayerAttackConfigSO _attackConfig;
    [SerializeField]
    private HitboxController _hitboxController;

    private float _comboReturnTime;

    private bool _attackBuffered;
    private bool _isAttacking;
    private bool _isSkillActive;

    private Coroutine _comboWindowCoroutine;
    private float _finisherTimer;

    private EAttackType _currentAttack;
    private int _currentCombo;
    private float _currentDamage;

    #region Life Cycle
    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<PlayerAnimator>();

        _playerRenderers = GetComponentsInChildren<Renderer>();

    }
    private void Start()
    {
        _player.OnPlayerStatsChanged += OnMovementStateChange;
        _playerMove.OnIsJumpingChanged += OnJumpingChange;
        _playerMove.OnDashEnd += StartJumpAttack;

        Initialized();
    }

    private void Update()
    {
        if (_isSkillActive) return;

        if (InputManager.Instance.GetKeyDown(EGameKeyType.Attack))
        {
            if (_currentAttack == EAttackType.None && _currentAttack == EAttackType.Jump) return;
            _attackBuffered = true;
        }
        
        if (!_isAttacking && _attackBuffered)
        {
            _attackBuffered = false;
            TryStartAttack();
        }
    }
    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnPlayerStatsChanged -= OnMovementStateChange;
        }

        if (_playerMove != null)
        {
            _playerMove.OnIsJumpingChanged -= OnJumpingChange;
        }
    }

    #endregion

    private void Initialized()
    {
        _moveState = _player.CurrentState;
        _isJumping = _playerMove.IsJumping;
        _comboReturnTime = _attackConfig.ComboReturnTime;
    }


    #region Attack

    private void TryStartAttack()
    {
        if (_moveState == EMovementState.Stagger)
        {
            return;
        }
        if (_playerMove.ShouldRun & _isJumping)
        {
            TryJumpAttack();
            return;
        }

        StartComboAttack(EAttackType.Basic);
    }

    private void TryJumpAttack()
    {
        _isAttacking = true;

        _currentAttack = EAttackType.Jump;

        _playerMove.StartGroundDash(_attackConfig.JumpDashAngle, _attackConfig.JumpDashSpeed);
        VisualHide();  
    }
    private void StartJumpAttack()
    {
        VisualShow();
        ExecuteSingleAttack(EAttackType.Jump, _attackConfig.JumpDashDamage);
        _currentCombo = 1;
    }

    private void StartComboAttack(EAttackType type)
    {
        _isAttacking = true;

        _currentAttack = EAttackType.Basic;

        _currentCombo = 1;
        PlayCurrentCombo();
    }

    private void PlayCurrentCombo()
    {
        var data = _attackConfig.GetPhaseData(_currentCombo);

        if (data == null)
        {
            EndCombo();
            return;
        }

        if (_comboTimerCoroutine != null)
            StopCoroutine(_comboTimerCoroutine);

        if (_currentCombo < _attackConfig.MaxPhaseCount)
        {
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(data.InputWindow));
            ExecuteComboAttack();
        }
        else
        {
            StartCoroutine(ComboFinisherCoroutine(_attackConfig.ChargeTime));
        }
    }


    private IEnumerator ComboFinisherCoroutine(float chargeTime)
    {
        _finisherTimer = 0;

        while (InputManager.Instance.GetKey(EGameKeyType.Attack))
        {
            _finisherTimer += Time.deltaTime;
            if (_finisherTimer > chargeTime)
            {
                //차지 피니셔.
                ExecuteChargeFinisherAttack();
                yield break;
            }
            yield return null;
        }
        //일반 콤보 피니셔.
        ExecuteComboAttack();
        Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
        yield return null;
    }

    //차지 피니셔 실행.
    private void ExecuteChargeFinisherAttack()
    {
        _playerMove.SetCanMove(false);
        Debug.Log($"[Attack] Type: Basic | Charge Finisher | Damage: {_attackConfig.ChargeFinisherDamage}");
        _currentDamage = SetDamage(_attackConfig.ChargeFinisherDamage);
        _animator.PlayChargeFinisher();
    }

    //콤보 없는 단일 공격 실행 - 애니메이션 이벤트로 OnAttackFinish() 실행 필요.
    private void ExecuteSingleAttack(EAttackType type, float damage)
    {
        _playerMove.SetCanMove(false);
        Debug.Log($"[Attack] Type: {type} | Skill | Damage: {damage}");
        _currentDamage = SetDamage(damage);
        _animator.PlaySkill(type);
    }

    private void ExecuteComboAttack()
    {
        Debug.Log($"[Attack] Type: Basic | {_currentCombo} Combo | Damage: {_attackConfig.Damage}");
        _currentDamage = SetDamage(_attackConfig.Damage);
        _animator.PlayComboAttack(_currentCombo);
    }

    private float SetDamage (float damage)
    {
        return damage;
    }

    

    #endregion

    #region Combo
    private IEnumerator ComboTimerCoroutine(float time)
    {
        float t = 0f;

        while (t < time)
        {
            if (_attackBuffered)
            {
                _attackBuffered = false;
                GoNextCombo();
                yield break;
            }

            t += Time.deltaTime;
            yield return null;
        }

        Debug.Log("[Attack] 콤보 타이머 만료 - 초기화");
        EndCombo();
    }

    private void GoNextCombo()
    {
        _currentCombo++;

        PlayCurrentCombo();
    }

    private void EndCombo()
    {
        _isAttacking = false;
        _attackBuffered = false;

        if (_comboWindowCoroutine != null)
        {
            StopCoroutine(_comboWindowCoroutine);
            _comboWindowCoroutine = null;
        }

        _currentCombo = 0;
        _currentAttack = EAttackType.None;

        _hitboxController.Deactivate("Main");
    }

    #endregion

    public void VisualHide()
    {
        _dashVFX.Play();
        foreach (Renderer renderer in _playerRenderers)
        {
            renderer.enabled = false;
        }
    }

    public void VisualShow()
    {
        _dashVFX.Play();
        foreach (Renderer renderer in _playerRenderers)
        {
            renderer.enabled = true;
        }
    }

    #region Public Method
    public void SetSkillActivate()
    {
        if (_currentAttack == EAttackType.None)
        {
            if (_comboTimerCoroutine != null)
            {
                StopCoroutine(_comboTimerCoroutine);
            }
            else
            {
                OnAttackFinish();
            }
        }

        _isSkillActive = true;
    }

    public void SetSkillDeactivate()
    {
        if (_comboTimerCoroutine != null)
        {
            StartCoroutine(ComboTimerCoroutine(_comboReturnTime));
        }
        _isSkillActive = false;
    }

    #endregion

    #region Value Change Event Method
    private void OnJumpingChange(bool value)
    {
        _isJumping = value;

        // 점프 스킬 중에는 리셋 콤보 무시.
        if (_currentAttack == EAttackType.Jump)
        {
            _currentAttack = EAttackType.Basic;
            return;
        }
        EndCombo();
    }

    private void OnMovementStateChange(EMovementState state)
    {
        _moveState = state;
    }

    #endregion

    #region Animation Event

    public void AttackStart()
    {
        _hitboxController.Activate("Main");
        //데미지 값 세팅
    }

    public void OnFinisherFinish()
    {
        //움직일 수 있는 상태로 전환
        _playerMove.SetCanMove(true);
        EndCombo();
    }

    public void OnJumpDashAttackFinish()
    {
        _hitboxController.Deactivate("Main");
        //움직일 수 있는 상태로 전환
        _playerMove.SetCanMove(true);
        _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_attackConfig.JumpDashComboInputWindow));

        _attackBuffered = false;
    }

    public void OnAttackFinish()
    {
        _hitboxController.Deactivate("Main");
        //움직일 수 있는 상태로 전환
        _playerMove.SetCanMove(true);

        _isAttacking = false;
        _attackBuffered = false;
    }

    #endregion
}
