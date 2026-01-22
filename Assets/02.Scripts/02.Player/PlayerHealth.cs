using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Button]
    public void PlayerDead()
    {
        _stats.Health.SetCurrent(0);


        if (_stats.Health.IsEmpty())
        {
            _animator.SetDieTrigger();
            _stateMachine.SetState(EPlayerState.Dead);
        }
    }
    public ETeamType Team => ETeamType.Player;

    private PlayerStats _stats;
    private PlayerStateMachine _stateMachine;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;

    public void ApplyDamage(DamageData data)
    {
        if (_stateMachine.CurrentState == EPlayerState.Dead || _stateMachine.CurrentActionState == EActionState.Skill) return;

        
        _stats.Health.Consume(_stats.CalculateReceivedDamage(data));

        
        if (_stats.Health.IsEmpty())
        {
            _animator.SetDieTrigger();
            _stateMachine.SetState(EPlayerState.Dead);
        }
        else
        {
            _playerMove.KnockBack(data.HitDirection);
            _animator.SetHitTrigger();
        }


        //Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator = GetComponent<PlayerAnimator>();
        _stateMachine = GetComponent<PlayerStateMachine>();
        _playerMove = GetComponent<PlayerMove>();
    }
}
