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
    private PlayerAttackConfigSO _attackConfig;
    [SerializeField]
    private HitboxController _hitboxController;

    private bool _attackBuffered;
    private bool _isAttacking;
    private Coroutine _comboWindowCoroutine;

    private PlayerAttackConfigSO __pausedComboConfig;
    private AttackTypeConfig _currentAttackConfig;
    private int _currentCombo;

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

        //콤보 중인 스킬이 있다면 코루틴을 중지시키고 _pausedComboConfig 에 저장 후 스킬이 끝나면 복구 시킨 후 다시 코루틴(0.5초)를 실행
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
            ExecuteJumpAttack();
            return;
        }

        StartCombo(EAttackType.Basic);
    }
    private void StartCombo(EAttackType type)
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

        ExecuteAttack(data, _currentAttackConfig.AttackType, _currentAttackConfig.Damage);

        if (_comboTimerCoroutine != null)
            StopCoroutine(_comboTimerCoroutine);

        if (_currentCombo < _currentAttackConfig.MaxPhaseCount)
        {
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(data.InputWindow));
        }
        else
        {
            Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
            // 마지막 타는 애니메이션 종료 이벤트에서 EndCombo
            EndCombo();
        }
    }

    private void ExecuteAttack(AttackPhaseData data, EAttackType type, DamageData damage)
    {
        Debug.Log($"[Attack] Type: {type} | PhaseIndex: {data.PhaseIndex} | Damage: {damage.Damage}");

        _animator.PlayAttack(data.PhaseIndex, type);
    }

    private void ExecuteJumpAttack()
    {
        ExecuteAttackSingle(EAttackType.Jump);
        _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_currentAttackConfig.GetPhaseData(1).InputWindow));
    }

    private void ExecuteAttackSingle(EAttackType attackType)
    {
        _isAttacking = true;

        _currentAttackConfig = _attackConfig.GetAttackConfig(attackType);
        if (_currentAttackConfig == null) return;


        AttackPhaseData data = _currentAttackConfig.GetPhaseData(1);

        if (data == null)
        {
            return;
        }

        ExecuteAttack(data, attackType, _currentAttackConfig.Damage);
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

        _animator.ResetCombo();
        _hitboxController.DeActive("Main");
    }

    #endregion

    #region Value Change Event Method
    private void OnJumpingChange(bool value)
    {
        _isJumping = value;

        // 점프 스킬 중에는 리셋 콤보 무시.
        if (_currentAttackConfig != null && _currentAttackConfig.AttackType == EAttackType.Jump)
        {
            _currentAttackConfig = _attackConfig.GetAttackConfig(EAttackType.Basic);
            GoNextCombo();
            return;
        }
        _animator.ResetCombo();
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
    }

    public void OnSkillFinish()
    {
        _currentAttackConfig = null;
        _hitboxController.DeActive("Main");
    }

    public void OnBasicAttackFinish()
    {
        _hitboxController.DeActive("Main");
    }

    public void OnComboFinisherFinish()
    {
        _hitboxController.DeActive("Main");
        EndCombo();
    }

    #endregion
}
