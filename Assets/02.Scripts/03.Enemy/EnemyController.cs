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
        //현재 정지 상태 혹은 사망상태라면 Fsm틱을 진행하지 않음
        if (_isPaused || FSM.CurrentState is DeadState)
        {
            return;
        }

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

    public void Dead()
    {
        SetDamageAcceptable(false);
        _fsm.Reset();
        // 물리 Collider 비활성화
        _physics.isKinematic = true;
        ReturnToPoolAfter(3f);
        OnDead?.Invoke(this);
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Subscribe(HandleBattleState);
            HandleBattleState(BattleManager.Instance.State);
        }
            
    }

    public override void OnDespawn()
    {
        if (BattleManager.Instance != null)
            BattleManager.Instance.OnBattleStateChanged.Unsubscribe(HandleBattleState);

        base.OnDespawn();
    }

    private void HandleBattleState(EBattleState state)
    {
        switch (state)
        {
            case EBattleState.Paused:
                Pause();
                break;
            case EBattleState.WaitingNextStage:
                break;
            default:
                Resume();
                break;
        }
    }



    [Button]
    public void Pause()
    {
        _isPaused = true;
        _move?.StopMove();
        _anim?.SetAnimSpeed(0f);
    }
    [Button]
    public void Resume()
    {
        _isPaused = false;
        _move?.StartMove();
        _anim?.SetAnimSpeed(1f);
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

        int dir = DirectionConvert(data.HitDirection);
        Anim.SetInt(AnimatorController.s_hitDirInt, dir);

        if (_health.IsDead)
        {
            HandleDead();
        }
        else
        {
            HandleDamaged();
        }

        Debug.Log($"{gameObject.name} 피격, {data.AttackId}, {data.HitDirection}, {dir}");
    }

    private int DirectionConvert( Vector3 hitDirection)
    {
        Vector3 localDir = transform.InverseTransformDirection(hitDirection);
        localDir.y = 0f;

        // Decide by dominant axis
        if (Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z))
        {
            return localDir.x < 0f? (int)EHitDirection.Right : (int)EHitDirection.Left;
        }

        return localDir.z < 0f ? (int)EHitDirection.Front : (int)EHitDirection.Back;
    }

    [Button]
    public void HandleDamaged()
    {
        if (FSM.CurrentState is DeadState)
        {
            return;
        }

        _fsm.ChangeState(EEnemyState.Hit);
    }

    [Button]
    public void HandleDead()
    {
        if (FSM.CurrentState is DeadState)
        {
            return;
        }

        _fsm.ChangeState(EEnemyState.Dead);
    }
    #endregion

    #region 애니메이션 이벤트용
    [Button]
    public void HitRecover()
    {
        Debug.Log($"[HitRecover] frame:{Time.frameCount}, state:{FSM.CurrentState}");
        FSM.ChangeState(EEnemyState.Idle);
        _anim.SetTrigger(AnimatorController.s_resetTrigger);
    }

    public void OnBeginAttack()
    {
        Debug.Log($"[OnBeginAttack] frame:{Time.frameCount}, state:{FSM.CurrentState}");
        Attack.OnBeginAttack();
        StartLoopDelay(Attack.CurrentLoopDelay);
    }

    private Coroutine loopRoutine;
    private void StartLoopDelay(float delay)
    {
        if (delay <= 0f)
            return;

        StopLoopDelay();
        loopRoutine = StartCoroutine(LoopDelayRoutine(delay));
    }
   
    private IEnumerator LoopDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _anim.SetTrigger("AttackLoopEnd");
    }

    public void CancelAttack()
    {
        StopLoopDelay();
    }

    private void StopLoopDelay()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }
    }

    public void OnEndAttack()
    {
        Debug.Log($"[OnEndAttack] frame:{Time.frameCount}, state:{FSM.CurrentState}");
        _attack.OnEndAttack();
    }


    public void OnAttackComplete()
    {
        Debug.Log($"[OnAttackComplete] frame:{Time.frameCount}, state:{FSM.CurrentState}");
        StopLoopDelay();
        Attack.Finish();

        if (FSM.CurrentState is AttackState attackState)
        {
            attackState.OnAttackFinished();
        }
    }

    #endregion
}

