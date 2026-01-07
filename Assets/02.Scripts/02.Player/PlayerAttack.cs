using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;

    private EPlayerState _state;
    private bool _isJumping;

    private Coroutine _comboTimerCoroutine;

    [SerializeField] 
    private PlayerComboConfigSO _ComboConfig;

    private ComboData _currentComboData;
    private int _currentCombo;

    private void Start()
    {
        _player = GetComponent<Player>();
        _playerMove = GetComponent<PlayerMove>();
        _animator = GetComponent<PlayerAnimator>();

        _player.OnPlayerStatsChanged += OnStateChange;
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
            _player.OnPlayerStatsChanged -= OnStateChange;
        }

        if (_playerMove != null)
        {
            _playerMove.OnIsJumpingChanged -= OnJumpingChange;
        }
    }

    private void Initialized()
    {
        _state = _player.CurrentState;
        _isJumping = _playerMove.IsJumping;
    }

    private void TryAttack()
    {
        if (_state == EPlayerState.Skill)
        {
            return;
        }

        //_animator.SetAttackTrigger();
        if (_playerMove.ShouldRun && !_isJumping)
        {
            Debug.Log("대쉬공격");
            _playerMove.ShouldRun = false;
            _currentCombo ++;
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(0.3f));
        }
        else
        {
            EAttackType attackType = _isJumping ? EAttackType.Air : EAttackType.Ground;
            ExecuteAttackCombo(attackType);
        }       
    }

    private void ExecuteAttackCombo(EAttackType attackType)
    {
        AttackTypeConfig attackConfig = _ComboConfig.GetAttackConfig(attackType);
        if (attackConfig == null)
        {
            return;
        }

        if (_comboTimerCoroutine != null)
        {
            StopCoroutine(_comboTimerCoroutine);
        }

        _currentCombo++;

        _currentComboData = attackConfig.GetComboData(_currentCombo);

        ExecuteAttack(_currentComboData, attackType);

        if (_currentCombo < attackConfig.MaxComboCount)
        {
            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine(_currentComboData.InputWindow));
        }
        else
        {
            Debug.Log($"[Attack] 콤보 피니셔 {_currentCombo}타");
            ResetCombo();
        }

    }

    private void ExecuteAttack(ComboData data, EAttackType type)
    {
        Debug.Log($"Attack - [{type}] Combo : {data.ComboIndex} Damage: {data.Damage}");
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
        _currentComboData = null;
    }

    private void OnJumpingChange(bool value)
    {
        _isJumping = value;
        ResetCombo();
    }

    private void OnStateChange(EPlayerState state)
    {
        _state = state;
    }  
}
