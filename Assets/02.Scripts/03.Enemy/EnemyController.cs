using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

//전체적인 조작을 담당.
//State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField] private ETeamType _team;

    [SerializeField] private Rigidbody _physics;
    [SerializeField] private Transform _target;

    [ShowInInspector] private EnemyStateMachine _fsm;
    

    [SerializeField] private EnemyMove _move;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private AnimatorController _anim;

    [SerializeField] private bool _isPaused;

    public EnemyStateMachine FSM => _fsm;
    public EnemyMove Move => _move;
    public EnemyAttack Attack => _attack;
    public EnemyHealth Health => _health;
    public EnemyStat Stat => _stat;
    public AnimatorController Anim => _anim;
    public Transform Target => _target;
    public ETeamType Team => _team;

    public SafeEvent<EnemyController> OnDead = new();
    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
        _health = GetComponent<EnemyHealth>();
        _move = GetComponent<EnemyMove>();
        _attack = GetComponent<EnemyAttack>();
        _anim = GetComponent<AnimatorController>();

        _physics = GetComponent<Rigidbody>();
        _fsm = new EnemyStateMachine(this);
    }

    private void Update()
    {
        if (_isPaused)
            return;

        _fsm.Tick(Time.deltaTime);
    }


    #region 생명주기
    [Button]
    public void Init()
    {
        _physics.isKinematic = false;

        _stat.Init();
        _health.Init();
        _move.Init();
        _attack.Init();
        _anim.Init();

        _fsm.Reset();
        _fsm.ChangeState(EEnemyState.Idle);

        SetDamageAcceptable(true);
    }

    [Button]
    public void Dead()
    {
        SetDamageAcceptable(false);
        _fsm.Reset();

        _move.PauseAgent();
        CancelAttack();
        _anim.SetTrigger(AnimatorController.s_deadTrigger);

        // 물리 Collider 비활성화
        _physics.isKinematic = true;
        ReturnToPoolAfter(3f);

        OnDead?.Invoke(this);
    }

    [Button]
    public void Pause()
    {
        _isPaused = true;
        _move.StopMove();
        _anim.SetAnimSpeed(0f);
    }
    [Button]
    public void Resume()
    {
        _isPaused = false;
        _move.StartMove();
        _anim.SetAnimSpeed(1f);
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
        _health.SetDamageable(enable);
    }

    public void ApplyDamage(DamageData data)
    {
        if (!_health.TryApplyDamage(data.Damage))
        {
            return;
        }

        if (_health.IsDead)
        {
            HandleDead();
        }
        else
        {
            HandleDamaged(data);
        }

        Debug.Log($"{gameObject.name} 피격, {data.AttackId}");
    }

    [Button]
    private void HandleDamaged(DamageData data)
    {
        if (FSM.CurrentState is DeadState)
        {
            return;
        }

        _fsm.ChangeState(EEnemyState.Hit);
    }

    private void HandleDead()
    {
        _fsm.ChangeState(EEnemyState.Dead);
    }
    #endregion

    #region 애니메이션 이벤트용
    [Button]
    public void HitRecover()
    {
        FSM.ChangeState(EEnemyState.Idle);
        _anim.SetTrigger(AnimatorController.s_resetTrigger);
    }

    public void OnBeginAttack()
    {
        _attack.OnBeginAttack();

        StopLoopDelay();
        _loopDelayRoutine = StartCoroutine(LoopDelayRoutine(_attack.LoopDelay));
    }

    private Coroutine _loopDelayRoutine;
    private IEnumerator LoopDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_attack == null || !_attack.IsAttacking)
            yield break;

        _anim.SetTrigger("AttackLoopEnd");
    }

    public void CancelAttack()
    {
        StopLoopDelay();
        _attack.CancelAttack();
    }

    private void StopLoopDelay()
    {
        if (_loopDelayRoutine != null)
        {
            StopCoroutine(_loopDelayRoutine);
            _loopDelayRoutine = null;
        }
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

