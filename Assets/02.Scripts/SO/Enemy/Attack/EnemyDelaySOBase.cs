using UnityEngine;

public abstract class EnemyDelaySOBase : ScriptableObject
{
    [Header("Base")]
    [SerializeField] protected float _delay = 0f;
    public float Delay => _delay;

    public virtual void BeginLoop()
    {
        Debug.Log($"{_delay}뒤 공격 실행");
    }

    //실제 효과 발동
    public abstract void Execute(Transform spawnPos, float damage);

}

