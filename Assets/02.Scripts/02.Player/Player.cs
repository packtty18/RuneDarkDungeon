using System;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class Player : MonoBehaviour
{
    private PlayerStats _playerStats;
    private EPlayerState _currentState;

    public EPlayerState CurrentState => _currentState;

    
    void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        switch (_currentState)
        {
            case EPlayerState.Idle:

                break;
            case EPlayerState.Walk:

                break;
            case EPlayerState.Run:

                break;
            case EPlayerState.Jump:

                break;
            default:
                break;
        }
    }

    public void SetState(EPlayerState newState)
    {
        _currentState = newState;
        switch (_currentState)
        {
            case EPlayerState.Idle:

                break;
            case EPlayerState.Walk:

                break;
            case EPlayerState.Run:

                break;
            case EPlayerState.Jump:

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
