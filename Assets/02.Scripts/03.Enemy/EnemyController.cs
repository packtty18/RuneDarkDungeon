using Sirenix.OdinInspector;
using System;
using UnityEngine;

//전체적인 조작을 담당.
//State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField] private ETeamType _team;

    [SerializeField] private Rigidbody _physics;
    [SerializeField] private Transform _target;

    [ShowInInspector] private EnemyStateMachine _fsm;
    [SerializeField] private bool _damageAcceptable = false;
    public SafeEvent<EnemyController> OnDead = new();

    private EnemyMove _move;
    private EnemyAttack _attack;
    private EnemyHealth _health;
    private EnemyStat _stat;
    private AnimatorController _anim;

    public EnemyStateMachine FSM => _fsm;
    public EnemyMove Move => _move;
    public EnemyAttack Attack => _attack;
    public EnemyHealth Health => _health;
    public EnemyStat Stat => _stat;
    public AnimatorController Anim => _anim;
    public Transform Target => _target;
    public ETeamType Team => _team;


    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
        _health = GetComponent<EnemyHealth>();
        _move = GetComponent<EnemyMove>();
        _attack = GetComponent<EnemyAttack>();
        _anim = GetComponent<AnimatorController>();

        _physics = GetComponent<Rigidbody>();
        _fsm = new EnemyStateMachine();
    }

    private void Update()
    {
        _fsm.Update();
    }

    #region 생명주기
    [Button]
    public void Init()
    {
        _physics.isKinematic = false;

        _fsm.Reset();
        _fsm.ChangeState(new IdleState(this));

        _stat.Init();
        _health.Init();
        _move.Init();
        _attack.Init();
        _anim.Init();

        SetDamageAcceptable(true);
    }

    [Button]
    public void Dead()
    {
        SetDamageAcceptable(false);

        _move.PauseAgent();
        _attack.CancelAttack();
        _anim.SetTrigger(AnimatorController.s_triggerDead);

        // 물리 Collider 비활성화
        _physics.isKinematic = true;
        ReturnToPoolAfter(3f);

        OnDead?.Invoke(this);
    }

    #endregion

    #region Target
    public void SetTarget(Transform target)
    {
        if (target == null)
        {
            return;
        }
        _target = target;
    }

    public bool IsTargetExist()
    {
        return _target != null;
    }

    public Vector3 GetTargetPosition()
    {
        if(!IsTargetExist())
        {
            //타겟이 없을때 호출되면 자기자신을 반환
            return transform.position;
        }
        return _target.position;
    }

    public bool IsTargetInRange(float range)
    {
        if (_target == null)

        {
            return false;
        }

       float sqrDistance = (transform.position - _target.position).sqrMagnitude;

        return sqrDistance <= range * range;
    }

    #endregion

    #region Health관련
    private void SetDamageAcceptable(bool enable)
    {
        _damageAcceptable = enable;
    }

    public void ApplyDamage(DamageData data)
    {
        if(!_damageAcceptable)
        {
            return;
        }

        _health.DecreaseHealth(data.Damage);

        if (_health.IsHealthEmpty())
        {
            HandleDead();
        }
        else
        {
            HandleDamaged(data);
        }

        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    private void HandleDamaged(DamageData data)
    {
        if (FSM.CurrentState is DeadState)
        {
            return;
        }

        _fsm.ChangeState(new HitState(this));
    }

    private void HandleDead()
    {
        _fsm.ChangeState(new DeadState(this));
    }
    #endregion

    #region 애니메이션 이벤트용
    public void HitRecover()
    {
        FSM.ChangeState(new IdleState(this));
        _anim.SetTrigger(AnimatorController.s_triggerReset);
    }

    public void OnBeginAttack()
    {
        _attack.OnBeginAttack();

        if(_attack.LoopDelay >0 )
        {
            Invoke("OnLoopEnd", _attack.LoopDelay);
        }
    }

    //공격 애니메이션의 Loop를 종료
    private void OnLoopEnd()
    {
        if (!_attack.IsAttacking)
        {
            return;
        }

        _anim.SetTrigger("AttackLoopEnd");
    }

    public void OnEndAttack()
    {
        _attack.OnAttackEnd();
    }

    public void OnAttackComplete()
    {
        _attack.OnAttackComplete();
    }

    #endregion
}

