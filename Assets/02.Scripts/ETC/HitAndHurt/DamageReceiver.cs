using UnityEngine;

/*
 * 한 객체에 한번의 공격에서 여러개의 hurtbox가 맞을 경우 최초의 1개만 처리.
 */
public class DamageReceiver : MonoBehaviour
{
    private int _lastAttackId = -1;
    private IDamageable _damageable;

    public SafeEvent<HurtBox> OnDamagedEvent = new SafeEvent<HurtBox>();

    private void Awake()
    {
        _damageable = GetComponentInParent<IDamageable>();
        if (_damageable == null)
        {
            Debug.LogError("IDamageable not found on DamageReceiver owner.");
        }
    }

    public void ReceiveDamage(DamageData data,HurtBox hurtbox)
    {
        if (_lastAttackId == data.AttackId)
        {
            return;
        }

        _lastAttackId = data.AttackId;
        _damageable.ApplyDamage(data);

        OnDamagedEvent?.Invoke(hurtbox);
    }
}
