using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private EMovementState _currentState;
    private EActionState _currentActionState;

    public EMovementState CurrentState => _currentState;
    public EActionState CurrentActionState => _currentActionState;

    public Action<EMovementState> OnPlayerStatsChanged;
    public Action<EActionState> OnPlayerActionChanged;
    public event Action<bool> OnCanMoveChanged;

    void Awake()
    {
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
            case EActionState.None:
                OnCanMoveChanged?.Invoke(true);
                break;
            default:
                OnCanMoveChanged?.Invoke(false);
                break;
        }
    }
}
