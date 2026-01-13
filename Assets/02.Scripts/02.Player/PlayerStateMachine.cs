using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private EPlayerState _currentState;
    private EActionState _currentActionState;

    public EPlayerState CurrentState => _currentState;
    public EActionState CurrentActionState => _currentActionState;

    public Action<EPlayerState> OnPlayerStatsChanged;
    public Action<EActionState> OnPlayerActionChanged;
    public event Action<bool> OnCanMoveChanged;

    void Awake()
    {
        _currentState = EPlayerState.None;
        _currentActionState = EActionState.None;
    }

    public bool CanReceiveMoveInput()
    {
        return CurrentState != EPlayerState.Dead
            && CurrentActionState != EActionState.Skill
            && CurrentActionState != EActionState.Dodge
            && CurrentActionState != EActionState.DashAttack
            && CurrentActionState != EActionState.Finisher;
    }

    public bool CanReceiveSkillInput()
    {
        return CurrentState != EPlayerState.Dead
            && CurrentActionState != EActionState.Skill
            && CurrentActionState != EActionState.Dodge
            && CurrentActionState != EActionState.DashAttack;
    }

    public void SetState(EPlayerState newState)
    {
        _currentState = newState;

        OnPlayerStatsChanged?.Invoke(_currentState);

        switch (_currentState)
        {
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
            case EActionState.None:
                OnCanMoveChanged?.Invoke(true);
                break;
            default:
                OnCanMoveChanged?.Invoke(false);
                break;
        }
    }
}
