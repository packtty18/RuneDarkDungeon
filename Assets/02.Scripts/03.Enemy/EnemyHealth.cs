using UnityEngine;

//Health스텟 조절 및 사망 혹은 히트 이벤트 발동
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]private ETeamType _team;
    public ETeamType Team => _team;

    public EnemyFacade _enemyFacade;
    public IReadOnlyConsumable<float> _health => _enemyFacade.Stat.GetValue(EEnemyConsumableFloat.Health);

    [SerializeField] private bool _damageAcceptable = false;

    public void Init()
    {
        _enemyFacade = GetComponent<EnemyFacade>();

        _damageAcceptable = true;
    }

    public void ApplyDamage(DamageData data)
    {
        if (!_damageAcceptable)
        {
            return;
        }

        _health.Consume(data.Damage);
        if(_health.IsEmpty())
        {
            //사망
        }
        else
        {
            //경직
        }


        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    public void SetDamageAcceptable(bool value)
    {
        _damageAcceptable = value;
    }
}
