using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player _player;
    private PlayerAnimator _animator;
    private EPlayerState _state;

    private void Start()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<PlayerAnimator>();

        _player.OnPlayerStatsChanged += StateChange;

        Initialized();
    }

    private void StateChange(EPlayerState state)
    {
        _state = state;
    }

    private void Initialized()
    {
        _state = _player.CurrentState;
    }

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(EGameKeyType.Attack))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (_state == EPlayerState.Skill)
        {
            return;
        }

        _animator.SetAttackTrigger();
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnPlayerStatsChanged -= StateChange;
        }
    }
}
