using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;
    [SerializeField]
    private HitBox _hitBox;

    private EMovementState _moveState;

    private bool _isJumping;

    private Coroutine _comboTimerCoroutine;

    [SerializeField] 
    private PlayerAttackConfigSO _attackConfig;
    [SerializeField]
    private HitboxController _hitboxController;

    private bool _attackBuffered;
    private bool _isAttacking;
    private Coroutine _comboWindowCoroutine;
    private float _finisherTimer;

    private PlayerAttackConfigSO __pausedComboConfig;
    private AttackTypeConfig _currentAttackConfig;
    private int _currentCombo;
    private float _currentDamage;

    #region Life Cycle
    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<PlayerAnimator>();
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
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Attack))
        {
            if (_currentAttackConfig != null && _currentAttackConfig.AttackType == EAttackType.Jump) return;
            _attackBuffered = true;
        }

        if (!_isAttacking && _attackBuffered)
        {
            _attackBuffered = false;
            TryStartAttack();
        }

        //콤보 중인 스킬이 있다면 코루틴을 중지시키고 _pausedComboConfig 에 저장 후 스킬이 끝나면 복구 시킨 후 다시 코루틴(0.5초)를 실행.
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
        _playerMove.StartGroundDash(30, 50);
    }
    private void StartJumpAttack()
    {
        StartSingleAttack(EAttackType.Jump);
        _currentCombo = 1;
        _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_currentAttackConfig.GetPhaseData(1).InputWindow));
    }

    private void StartSingleAttack(EAttackType attackType)
    {
        _isAttacking = true;

        _currentAttackConfig = _attackConfig.GetAttackConfig(attackType);
        if (_currentAttackConfig == null) return;

        ExecuteSingleAttack(attackType, _currentAttackConfig.Damage);
    }

    private void StartComboAttack(EAttackType type)
    {
        _isAttacking = true;

        _currentAttackConfig = _attackConfig.GetAttackConfig(type);
        if (_currentAttackConfig == null) return;

        _currentCombo = 1;
        PlayCurrentCombo();
    }

    private void PlayCurrentCombo()
    {
        var data = _currentAttackConfig.GetPhaseData(_currentCombo);

        if (data == null)
        {
            EndCombo();
            return;
        }

        if (_comboTimerCoroutine != null)
            StopCoroutine(_comboTimerCoroutine);

        if (_currentCombo < _currentAttackConfig.MaxPhaseCount)
        {
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(data.InputWindow));
            ExecuteComboAttack(data, _currentAttackConfig.AttackType, _currentAttackConfig.Damage);
        }
        else
        {
            StartCoroutine(ComboFinisherCoroutine(_attackConfig.ChargeTime, data));
        }
    }


    private IEnumerator ComboFinisherCoroutine(float chargeTime, AttackPhaseData data)
    {
        _finisherTimer = 0;

        while (InputManager.Instance.GetKey(EGameKeyType.Attack))
        {
            _finisherTimer += Time.deltaTime;
            if (_finisherTimer > chargeTime)
            {
                //차지 피니셔.
                ExecuteChargeFinisherAttack(_currentAttackConfig.AttackType, _attackConfig.ChargeFinisherDamage);
                yield break;
            }
            yield return null;
        }
        //일반 콤보 피니셔.
        ExecuteComboAttack(data, _currentAttackConfig.AttackType, _currentAttackConfig.Damage);
        Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
        yield return null;
    }

    //차지 피니셔 실행.
    private void ExecuteChargeFinisherAttack(EAttackType type, float damage)
    {
        _playerMove.SetCanMove(false);
        Debug.Log($"[Attack] Type: {type} | Charge Finisher | Damage: {damage}");
        _currentDamage = SetDamage(damage);
        _animator.PlayChargeFinisher(type);
    }

    //단일 전신 스킬 실행 - 애니메이션 이벤트로 OnAttackFinish() 실행 필요.
    private void ExecuteSingleAttack(EAttackType type, float damage)
    {
        _playerMove.SetCanMove(false);
        Debug.Log($"[Attack] Type: {type} | Skill | Damage: {damage}");
        _currentDamage = SetDamage(damage);
        _animator.PlaySkill(type);
    }

    private void ExecuteComboAttack(AttackPhaseData data, EAttackType type, float damage)
    {
        Debug.Log($"[Attack] Type: {type} | PhaseIndex: {data.PhaseIndex} | Damage: {damage}");
        _currentDamage = SetDamage(damage);
        _animator.PlayComboAttack(data.PhaseIndex, type);
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
        _currentAttackConfig = null;

        _hitboxController.DeActive("Main");
    }

    #endregion

    #region Value Change Event Method
    private void OnJumpingChange(bool value)
    {
        _isJumping = value;

        // 점프 스킬 중에는 리셋 콤보 무시.
        if ( _currentAttackConfig != null && _currentAttackConfig.AttackType == EAttackType.Jump)
        {
            _currentAttackConfig = _attackConfig.GetAttackConfig(EAttackType.Basic);
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
        _hitboxController.Active("Main");
        //데미지 값 세팅
    }

    public void OnFinisherFinish()
    {
        //움직일 수 있는 상태로 전환
        _playerMove.SetCanMove(true);
        EndCombo();
    }

    public void OnAttackFinish()
    {
        _hitboxController.DeActive("Main");
        //움직일 수 있는 상태로 전환
        _playerMove.SetCanMove(true);
    }

    #endregion
}
