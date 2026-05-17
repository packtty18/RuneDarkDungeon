using Drakkar.GameUtils;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStateController _stateController;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;
    private PlayerStats _stats;
    private PlayerSound _sound;
    private GroundEffectSpawner _spawner;

    private Coroutine _comboTimerCoroutine;

    private Renderer[] _playerRenderers;

    private EffectPlayer _dashVFX;
    private EffectPlayer _dashSlashVFX;
    private EffectPlayer _finisherVFX;
    private EffectPlayer _finisherSlashVFX;
    private EffectPlayer[] _slashVFXs;

    [SerializeField]
    private Transform _swordPosition;
    [SerializeField] 
    private PlayerAttackConfigSO _attackConfig;
    [SerializeField]
    private HitboxController _hitboxController;
    [SerializeField]
    private float _finisherShakeTime = 0.5f;
    [SerializeField]
    private float _finisherShakeAmplitude = 3;

    private float _comboReturnTime;

    private bool _attackBuffered;
    private bool _isAttacking;

    private Coroutine _comboWindowCoroutine;
    private float _finisherTimer;

    private EAttackType _currentAttack;
    private int _currentCombo;
    private float _currentDamage;

    public event Action<float, float> OnComboChange;
    public event Action<bool> OnComboCharging;

    #region Life Cycle
    private void Awake()
    {
        _stateController = GetComponent<PlayerStateController>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<PlayerAnimator>();
        _stats = GetComponent<PlayerStats>();
        _spawner = GetComponent<GroundEffectSpawner>();
        _sound = GetComponent<PlayerSound>();

        _playerRenderers = GetComponentsInChildren<Renderer>();
    }
    private void Start()
    {
        _playerMove.OnIsJumpingChanged += OnJumpingChange;
        _playerMove.OnDashEnd += StartJumpAttack;
        EffectInstantiate();

        Initialized();
    }

    private void Update()
    {
        if (!_stateController.CanReceiveMoveInput()) return;

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

        if (_playerMove != null)
        {
            _playerMove.OnIsJumpingChanged -= OnJumpingChange;
        }
    }

    #endregion

    private void Initialized()
    {
        _comboReturnTime = _attackConfig.ComboReturnTime;
    }

    private void EffectInstantiate()
    {
        EffectPlayer[] vfx = _attackConfig.GetComboSlashVFXs();
        _slashVFXs = new EffectPlayer[vfx.Length];
        for (int i = 0;  i < vfx.Length; i++) {

            _slashVFXs[i] = Instantiate(vfx[i], _swordPosition);
        }
        _dashVFX = Instantiate(_attackConfig.JumpDashEffect, transform);
        _finisherVFX = Instantiate(_attackConfig.FinisherEffect, transform);
        _dashSlashVFX = Instantiate(_attackConfig.JumpDashSlashEffect, _swordPosition);
        _finisherSlashVFX = Instantiate(_attackConfig.FinisherSlashVFX, _swordPosition);
    }


    #region Attack

    private void TryStartAttack()
    {
        if (_playerMove.ShouldRun & _playerMove.IsJumping())
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

        _stateController.SetActionState(EActionState.DashAttack);
        _playerMove.StartGroundDash(_attackConfig.JumpDashAngle, _attackConfig.JumpDashSpeed);
        VisualHide();
        _sound.OnDash();
        ExecuteSingleAttack(EAttackType.Jump, _attackConfig.JumpDashDamage);
        _currentCombo = 1;
        OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);
    }
    private void StartJumpAttack()
    {
        VisualShow();
        /*ExecuteSingleAttack(EAttackType.Jump, _attackConfig.JumpDashDamage);
        _currentCombo = 1;
        OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);*/

    }

    private void StartComboAttack(EAttackType type)
    {
        _isAttacking = true;

        _currentAttack = EAttackType.Basic;

        _currentCombo = 1;
        OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);
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
            ExecuteComboAttack();
            OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);
        }
        else
        {
            StartCoroutine(ComboFinisherCoroutine(_attackConfig.ChargeTime));
        }
    }


    private IEnumerator ComboFinisherCoroutine(float chargeTime)
    {
        _finisherTimer = 0;
        OnComboCharging?.Invoke(false);

        while (true)
        {
            while (!_stateController.CanReceiveMoveInput())
            {
                yield return null;
            }

            if (!InputManager.Instance.GetKey(EGameKeyType.Attack))
            {
                break;
            }

            _finisherTimer += Time.deltaTime;

            if (_finisherTimer > chargeTime)
            {
                //차지 피니셔.
                ExecuteChargeFinisherAttack();
                OnComboCharging?.Invoke(true);
                yield break;
            }
            yield return null;
        }
        //일반 콤보 피니셔.
        ExecuteComboAttack();
        //Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
        OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);
        yield return null;
    }

    //차지 피니셔 실행.
    private void ExecuteChargeFinisherAttack()
    {
        _stateController.SetActionState(EActionState.Finisher);
        //Debug.Log($"[Attack] Type: Basic | Charge Finisher | Damage: {_attackConfig.ChargeFinisherDamage}");
        _currentDamage = _stats.CalculateDealDamage(_attackConfig.ChargeFinisherDamage);
        _animator.PlayChargeFinisher();
    }

    //콤보 없는 단일 공격 실행 - 애니메이션 이벤트로 OnAttackFinish() 실행 필요.
    private void ExecuteSingleAttack(EAttackType type, float damage)
    {
        //Debug.Log($"[Attack] Type: {type} | Skill | Damage: {damage}");
        _currentDamage = _stats.CalculateDealDamage(damage);
        _animator.PlaySingleAttack(type);
    }

    private void ExecuteComboAttack()
    {
        //Debug.Log($"[Attack] Type: Basic | {_currentCombo} Combo | Damage: {_attackConfig.Damage}");
        _currentDamage = _stats.CalculateDealDamage(_attackConfig.Damage);
        _animator.PlayComboAttack(_currentCombo);
    }    

    #endregion

    #region Combo
    private IEnumerator ComboTimerCoroutine(float time)
    {
        float t = 0f;

        while (t < time)
        {
            if (_attackBuffered || 
                (_currentCombo >= _attackConfig.MaxPhaseCount - 1 
                && InputManager.Instance.GetKey(EGameKeyType.Attack)))
            {
                _attackBuffered = false;
                GoNextCombo();
                yield break;
            }

            t += Time.deltaTime;
            yield return null;
        }

        //Debug.Log("[Attack] 콤보 타이머 만료 - 초기화");
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
        OnComboChange?.Invoke(_currentCombo, _attackConfig.MaxPhaseCount);

        _currentAttack = EAttackType.None;
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
    public void OnSkillInterrupt()
    {
        if (_currentAttack != EAttackType.None)
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

        _attackBuffered = false;

    }

    public void OnSkillComplete()
    {
        if (_comboTimerCoroutine != null)
        {
             StartCoroutine(ComboTimerCoroutine(_comboReturnTime));
        }
    }

    #endregion

    #region Value Change Event Method
    private void OnJumpingChange(bool value)
    {
        // 점프 스킬 중에는 리셋 콤보 무시.
        if (_currentAttack == EAttackType.Jump)
        {
            _currentAttack = EAttackType.Basic;
            return;
        }
        EndCombo();
    }

    #endregion

    #region Animation Event

    public void OnAttackStart()
    {
        _hitboxController.Activate("Main", _currentDamage);
    }

    public void OnChargeFinisherEffect()
    {
        _spawner.SpawnGroundEffect(_finisherVFX, _swordPosition.transform.position, transform, _attackConfig.ChargeFinisherDamage);
    }

    public void OnComboSlashVFX()
    {
        if (_currentCombo == 0) return;
        _slashVFXs[_currentCombo-1].Play();
    }

    public void OnJumpDashSlashVFX()
    {
        _dashSlashVFX.Play();
    }

    public void OnFinisherSlashVFX()
    {
        _finisherSlashVFX.Play();
    }

    public void OnAttackFinish()
    {
        _hitboxController.Deactivate("Main");

        if (_isAttacking)
        {
            float inputWindow = _attackConfig.GetPhaseData(_currentCombo).InputWindow;
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(inputWindow));
        }
    }
    public void OnSingleAttackFinish()
    {
        _hitboxController.Deactivate("Main");
        _stateController.SetActionState(EActionState.None);

        _isAttacking = false;
        _attackBuffered = false;
    }

    public void OnChargeFinisherFinish()
    {
        _hitboxController.Deactivate("Main");
        _stateController.SetActionState(EActionState.None);
        
        EndCombo();

        CameraManager.Instance.CameraShake(_finisherShakeAmplitude, _finisherShakeTime);
    }

    public void OnFinisherFinish()
    {
        _hitboxController.Deactivate("Main");
        _stateController.SetActionState(EActionState.None);
        
        EndCombo();
    }

    public void OnJumpDashAttackFinish()
    {
        _hitboxController.Deactivate("Main");
        _stateController.SetActionState(EActionState.None);

        _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_attackConfig.JumpDashComboInputWindow));

        _attackBuffered = false;
    }
    #endregion
}
