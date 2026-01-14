using UnityEngine;

//한 객체에 한번의 공격에서 여러개의 hurtbox가 맞을 경우 최초의 1개만 처리.
public class DamageReceiver : MonoBehaviour
{
    private int _lastAttackId = -1;
    private IDamageable _owner;

    public SafeEvent<HurtBox> OnDamagedEventEach = new SafeEvent<HurtBox>();
    public SafeEvent OnDamagedEvent = new SafeEvent();
    private void Awake()
    {
        _owner = GetComponentInParent<IDamageable>();
        if (_owner == null)
        {
            Debug.LogError("IDamageable not found on DamageReceiver owner.");
        }
    }

    public void ReceiveDamage(DamageData data, HurtBox hurtbox)
    {
       
        if (_lastAttackId == data.AttackId)
        {
            return;
        }

        //중립은 모든 객체
        //아군은 적군 혹은 중립
        //적군은 아군 혹은 중리
        if (data.Team != ETeamType.Neutral && _owner.Team == data.Team)
        {
            return;
        }


        _lastAttackId = data.AttackId;
        _owner.ApplyDamage(data);

        OnDamagedEventEach?.Invoke(hurtbox);
        OnDamagedEvent?.Invoke();
    }

    
}
