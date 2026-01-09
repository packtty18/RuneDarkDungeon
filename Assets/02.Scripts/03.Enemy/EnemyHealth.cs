using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
using UnityEngine;

//Health스텟 조절 및 사망 혹은 히트 이벤트 발동
public class EnemyHealth : MonoBehaviour
{
    [ShowInInspector, ReadOnly] private EnemyController _controller;
    [ShowInInspector, ReadOnly] private IReadOnlyConsumable<float> _health;

    private bool _canTakeDamage;

    public SafeEvent OnDead = new();
    public bool IsDead => _health.IsEmpty();

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    
    public void Init()
    {
        _health = _controller.Stat.GetValue(EEnemyConsumableFloat.Health);
        SetDamageable(true);
    }

    public bool TryApplyDamage(float damage)
    {
        if (!_canTakeDamage || _health.IsEmpty())
        {
            return false;
        }

        _health.Consume(damage);
        if (IsDead)
        {
            OnDead?.Invoke();
        }

        return true;
    }

    public void SetDamageable(bool enable)
    {
        _canTakeDamage = enable;
    }
}
