using UnityEngine;

public abstract class EnemyDelaySOBase : ScriptableObject
{
    [Header("Base")]
    [SerializeField] protected float _delay = 0f;
    public float Delay => _delay;

    //마법 사용시 장식 효과 생성?
    public virtual void BeginLoop()
    {
        Debug.Log($"[Summon] Windup Begin : {name}");
    }

    //실제 효과 발동
    public abstract void Execute(Transform spawnPos, float damage);

}

