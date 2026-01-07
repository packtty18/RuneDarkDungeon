using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyFacade : MonoBehaviour
{
    //적 스크립트에 접근하기 위함.
    //이후 절대로 직접 컴포넌트를 가져오지 않기
    [ShowInInspector] public EnemyMove Move { get; private set; }
    [ShowInInspector] public EnemyAttack Attack { get; private set; }
    [ShowInInspector] public EnemyHealth Health { get; private set; }
    [ShowInInspector] public EnemyStat Stat { get; private set; }
    [ShowInInspector] public AnimatorController Anim { get; private set; }

    private void Awake()
    {
        Move = GetComponent<EnemyMove>();
        Attack = GetComponent<EnemyAttack>();
        Health = GetComponent<EnemyHealth>();
        Stat = GetComponent<EnemyStat>();
        Anim = GetComponent<AnimatorController>();

        Debug.Log("[EnemyFacade] Initialized");
    }

    public void Init()
    {
        Stat.Init();
        Attack.Init();
        Health.Init();
        Move.Init();
        Anim.Init();
    }
}
