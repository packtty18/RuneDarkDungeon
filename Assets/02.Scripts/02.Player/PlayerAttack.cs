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

    private AttackTypeConfig _currentAttackConfig;
    private int _currentCombo;

    private void Start()
    {
        _player = GetComponent<Player>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<PlayerAnimator>();

        _player.OnPlayerStatsChanged += OnMovementStateChange;
        _playerMove.OnIsJumpingChanged += OnJumpingChange;

        Initialized();
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Attack))
        {
            TryAttack();
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

    private void Initialized()
    {
        _moveState = _player.CurrentState;
        _isJumping = _playerMove.IsJumping;
    }

    private void TryAttack()
    {
        if (_moveState == EMovementState.Stagger || (_currentAttackConfig != null && _currentAttackConfig.AttackType == EAttackType.Jump))
        {
            return;
        }
        if (_playerMove.ShouldRun & _isJumping)
        {
            ExecuteJumpAttack();
            return;
        }
        //_animator.SetAttackTrigger();

        ExecuteAttackCombo(EAttackType.Basic);
    }

    private void ExecuteJumpAttack()
    {
        ExecuteAttackSingle(EAttackType.Jump);
        _currentCombo++;
        _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_currentAttackConfig.GetPhaseData(1).InputWindow));
    }

    private void ExecuteAttackSingle(EAttackType attackType)
    {
        _currentAttackConfig = _attackConfig.GetAttackConfig(attackType);
        if (_currentAttackConfig == null)
        {
            return;
        }

        AttackPhaseData data = _currentAttackConfig.GetPhaseData(1);

        if (data == null)
        {
            return;
        }

        ExecuteAttack(data, attackType, _currentAttackConfig.Damage);
    }
    private void ExecuteAttackCombo(EAttackType attackType)
    {
        _currentAttackConfig = _attackConfig.GetAttackConfig(attackType);
        if (_currentAttackConfig == null)
        {
            return;
        }

        if (_comboTimerCoroutine != null)
        {
            StopCoroutine(_comboTimerCoroutine);
        }

        _currentCombo++;

        AttackPhaseData data = _currentAttackConfig.GetPhaseData(_currentCombo);

        if (data == null)
        {
            ResetCombo();
            return;
        }

        ExecuteAttack(data, attackType, _currentAttackConfig.Damage);

        if (_currentCombo < _currentAttackConfig.MaxPhaseCount)
        {
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(data.InputWindow));
        }
        else
        {
            Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
            ResetCombo();
        }

    }

    private void ExecuteAttack(AttackPhaseData data, EAttackType type, DamageData damage)
    {
        Debug.Log($"Attack - [{type}] Combo : {data.PhaseIndex} Damage: {damage.Damage}");
    }

    private IEnumerator ComboTimerCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        Debug.Log("[Attack] 콤보 타이머 만료 - 초기화");
        ResetCombo();
    }

    private void ResetCombo()
    {
        if ( _comboTimerCoroutine != null )
        {
            StopCoroutine(_comboTimerCoroutine);
            _comboTimerCoroutine = null;
        }
        _currentCombo = 0;
        _currentAttackConfig = null;
    }

    private void AttackFinish()
    {
        _currentAttackConfig = null;
    }

    private void SkillEnd()
    {

    }

    private void OnJumpingChange(bool value)
    {
        _isJumping = value;
        ResetCombo();
    }

    private void OnMovementStateChange(EMovementState state)
    {
        _moveState = state;
    }
}
