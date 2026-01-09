using System;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    private PlayerStats _playerStats;
    private EMovementState _currentState;
    private EActionState _currentActionState;

    public EMovementState CurrentState => _currentState;
    public EActionState CurrentActionState => _currentActionState;

    public Action<EMovementState> OnPlayerStatsChanged;
    public Action<EActionState> OnPlayerActionChanged;

    void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    public void SetState(EMovementState newState)
    {
        _currentState = newState;

        OnPlayerStatsChanged?.Invoke(_currentState);

        switch (_currentState)
        {
            case EMovementState.Idle:

                break;
            case EMovementState.Walk:

                break;
            case EMovementState.Run:

                break;
            case EMovementState.Jump:

                break;
            default:
                break;
        }
    }

    public void SetActionState(EActionState newState)
    {
        _currentActionState = newState;

        OnPlayerActionChanged?.Invoke(_currentActionState);

        switch (_currentActionState)
        {
            case EActionState.Attack:

                break;
            case EActionState.Skill:

                break;
            default:
                break;
        }
    }

    public float GetSpeed()
    {
        return _playerStats.MoveSpeed.Current;
    }

    public float GetGravity()
    {
        return _playerStats.Gravity.Value;
    }

    public float GetJumpVelocity()
    {
        return _playerStats.JumpPower.Value;
    }
    public void SubscribeSpeed(Action<float> action)
    {
        _playerStats.MoveSpeed.Subscribe(action);
    }
    public void UnsubscribeSpeed(Action<float> action)
    {
        _playerStats.MoveSpeed.Unsubscribe(action);
    }
}
