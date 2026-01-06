using Sirenix.OdinInspector;
using UnityEngine;

//해당 스크립트는 더이상 상속하지 않음!!
//RangedAttack과 MeleeAttack을 상속할것.
public class AttackBase : MonoBehaviour ,IEnemyAttack
{

    [SerializeField] private EAttackType _type;
    [SerializeField] protected string _name;
    [SerializeField] protected float _coolTime = 1f;
    [SerializeField] protected float _lastTime;
    
    public EAttackType AttackType => _type;
    public string Name => _name;
    [ShowInInspector] public bool CanExecute => Time.time >= _lastTime + _coolTime;
    public SafeEvent OnAttackFinished => new SafeEvent();

    //공격의 실행. 상속시 마지막에 base 넣기
    public virtual void Execute()
    {
        OnAttackFinished?.Invoke();
        _lastTime = Time.time;
    }

    //공격의 취소
    public virtual void Cancel()
    {
    }

}
