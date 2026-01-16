using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController controller;
    public abstract EEnemyState StateType { get; }

    protected EnemyState(EnemyController controller)
    {
        this.controller = controller;
    }

    public virtual void Enter()
    {
        //Debug.Log($"[FSM] Enter {GetType().Name}");
    }

    public virtual void Tick(float deltaTime) { }

    public virtual void Exit()
    {
        //Debug.Log($"[FSM] Exit {GetType().Name}");
    }
}