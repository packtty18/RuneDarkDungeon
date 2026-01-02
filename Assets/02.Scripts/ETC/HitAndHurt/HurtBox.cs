using UnityEngine;

//피격 부위를 식별.
[RequireComponent(typeof(Collider))]
public class HurtBox : MonoBehaviour
{
    private DamageReceiver _receiver;

    private void Awake()
    {
        _receiver = GetComponentInParent<DamageReceiver>();
        if (_receiver == null)
        {
            Debug.LogError("DamageReceiver not found in parent.");
        }
    }

    public void ApplyDamage(DamageData damage)
    {
        _receiver.ReceiveDamage(damage, this);
    }
}
