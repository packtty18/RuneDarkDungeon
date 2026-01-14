using System.Collections.Generic;
using UnityEngine;
public class EnemyStateMachine
{
    //생성된 스테이트를 버리지 않고 보관
    private readonly Dictionary<EEnemyState, EnemyState> _stateCache = new();
    private EnemyController _controller;
    [SerializeField] private EnemyState _currentState;

    public EnemyState CurrentState => _currentState;

    public EnemyStateMachine(EnemyController controller)
    {
        _controller = controller;
    }

    public void Reset()
    {
        _currentState?.Exit();
        _currentState = null;
    }
    public void ChangeState(EEnemyState newState)
    {
        _currentState?.Exit();

        _currentState = GetState(newState);
        _currentState.Enter();
    }

    private EnemyState GetState(EEnemyState type)
    {
        if (_stateCache.TryGetValue(type, out var state))
        {
            return state;
        }

        state = CreateState(type);
        _stateCache.Add(type, state);

        return state;
    }

    //새로운 상태 만들경우 추가
    private EnemyState CreateState(EEnemyState type)
    {
        Debug.Log($"[FSM] Create State : {type}");

        return type switch
        {
            EEnemyState.Idle => new IdleState(_controller),
            EEnemyState.Chase => new ChaseState(_controller),
            EEnemyState.Attack => new AttackState(_controller),
            EEnemyState.Hit => new HitState(_controller),
            EEnemyState.Dead => new DeadState(_controller),
            EEnemyState.Charge => new ChargeState(_controller),
            _ => null
        };
    }

    public void Tick(float deltaTime)
    {
        _currentState?.Tick(deltaTime);
    }
}