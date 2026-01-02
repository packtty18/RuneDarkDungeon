using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HurtBox : MonoBehaviour
{
    private DamageReceiver _receiver;
    private IDamageable _owner;

    private void Awake()
    {
        _receiver = GetComponentInParent<DamageReceiver>();
        _owner = GetComponentInParent<IDamageable>();

        if (_receiver == null)
        {
            Debug.LogError("DamageReceiver not found in parent.");
        }

        if (_owner == null)
        {
            Debug.LogError("IDamageable not found in parent.");
        }
    }

    public void ApplyDamage(DamageData damage)
    {
        // 팀 판정 (아군 공격 방지)
        if (_owner.Team == damage.Team && damage.Team != ETeamType.Neutral)
        {
            return;
        }

        _receiver.ReceiveDamage(damage, this);
    }
}
