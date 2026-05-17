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
            _stateController.SetState(EPlayerState.Dead);
        }
    }
    public ETeamType Team => ETeamType.Player;

    private PlayerStats _stats;
    private PlayerStateController _stateController;
    private PlayerAnimator _animator;
    private PlayerMove _playerMove;

    public void ApplyDamage(DamageData data)
    {
        if (_stateController.CurrentState == EPlayerState.Dead || _stateController.CurrentActionState == EActionState.Skill) return;

        
        _stats.Health.Consume(_stats.CalculateReceivedDamage(data));

        
        if (_stats.Health.IsEmpty())
        {
            _animator.SetDieTrigger();
            _stateController.SetState(EPlayerState.Dead);
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
        _stateController = GetComponent<PlayerStateController>();
        _playerMove = GetComponent<PlayerMove>();
    }
}
