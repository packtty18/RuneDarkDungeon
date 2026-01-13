using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public ETeamType Team => ETeamType.Player;

    private PlayerStats _stats;
    private PlayerStateMachine _stateMachine;
    private PlayerAnimator _animator;
    public void ApplyDamage(DamageData data)
    {
        if (_stateMachine.CurrentState == EPlayerState.Dead) return;

        float damage = data.Damage - (data.Damage * _stats.Defense.Current);
        Debug.Log($"받은 데미지 : {data.Damage} | 반영 데미지 : {damage}");
        _stats.Health.Consume(damage);

        
        if (_stats.Health.IsEmpty())
        {
            _animator.SetDieTrigger();
            _stateMachine.SetState(EPlayerState.Dead);
        }
        else
        {
            _animator.SetHitTrigger();
        }


        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator = GetComponent<PlayerAnimator>();
        _stateMachine = GetComponent<PlayerStateMachine>();
    }
}
