using System;
using UnityEngine;

//Health스텟 조절 및 사망 혹은 히트 이벤트 발동
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;

    private IReadOnlyConsumable<float> _health => _controller.Stat.GetValue(EEnemyConsumableFloat.Health);

    private void Awake()
    {
        _controller =GetComponent<EnemyController>();
    }
    public void Init()
    {
    }

    public void DecreaseHealth(float damage)
    {
        _health.Consume(damage);
    }

    public bool IsHealthEmpty()
    {
        return _health.IsEmpty();
    }
}
