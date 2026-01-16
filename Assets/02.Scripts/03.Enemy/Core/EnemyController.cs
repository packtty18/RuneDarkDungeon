
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


//전체적인 조작을 담당.
//State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField] private ETeamType _team;
    [SerializeField] private Rigidbody _physics;
    [SerializeField] private Transform _target;
    [SerializeField] private UIBase _healthUI;

    [ShowInInspector] private IEnemyBehavior _behavior;
    [ShowInInspector] private EnemyStateMachine _fsm;

    [SerializeField] private EnemyPhase _phase;
    [SerializeField] private EnemyMove _move;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private EnemyAnimator _anim;
    [SerializeField] private EnemyBuff _buff;
    
    
    [SerializeField] private bool _paused;
    
    [SerializeField] private bool _wait = false;
    


    public ETeamType Team => _team;
    public EnemyStateMachine FSM => _fsm;
    
    public IEnemyBehavior Behavior => _behavior; 

    public EnemyMove Move => _move;
    public EnemyAttack Attack => _attack;
    public EnemyHealth Health => _health;
    public EnemyStat Stat => _stat;
    public EnemyAnimator Anim => _anim;
    public EnemyBuff Buff => _buff;
    public EnemyPhase Phase => _phase;
    public bool Pause => _paused;
    public bool Wait => _wait;

    public Transform Target => _target;

    public SafeEvent<EnemyController> OnDead = new();
    private void Awake()
    {
        _stat = GetComponent<EnemyStat>();
        _health = GetComponent<EnemyHealth>();
        _move = GetComponent<EnemyMove>();
        _attack = GetComponent<EnemyAttack>();
        _anim = GetComponent<EnemyAnimator>();
        _buff= GetComponent<EnemyBuff>();
        _physics = GetComponent<Rigidbody>();
        _phase = GetComponent<EnemyPhase>();

        _stat.Init();
        _health.Init();
        _move.Init();
        _attack.Init();
        _anim.Init();
        _buff.Init();

        _behavior = CreateBehavior(_stat.EnemyType);

        _phase.Init();

        _fsm = new EnemyStateMachine(this);
    }
    public void SetConstraintsYPosition(bool enable)
    {
        if (enable)
        {
            _physics.constraints |= RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            _physics.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }

    private IEnemyBehavior CreateBehavior(EEnemyType type)
    {
        return type switch
        {
            EEnemyType.Warrior => new CommonEnemyBehavior(),
            EEnemyType.Archer => new CommonEnemyBehavior(),
            EEnemyType.Mage => new CommonEnemyBehavior(),
            EEnemyType.Elite => new EliteEnemyBehavior(),
            EEnemyType.Boss => new BossEnemyBehavior(),
            _ => new CommonEnemyBehavior()
        };
    }

    private void Update()
    {
        if (!CanTick())
        {
            return;
        }

        _fsm.Tick(Time.deltaTime);
    }
    private bool CanTick()
    {
        if (_paused)
            return false;

        if (FSM.CurrentState is DeadState)
            return false;

        return true;
    }

    #region 생명주기
    [Button]
    public void Init()
    {
        EnablePhysics(true);

        _stat.Init();
        _health.Init();
        _move.Init();
        _attack.Init();
        _anim.Init();
        _buff.Init();

        _behavior?.Initialize(this);
        _phase?.Reset();

        _fsm.Reset();
        _fsm.ChangeState(EEnemyState.Idle);

        _health.SetDamageable(true);

        
    }

    public void EnablePhysics(bool enable)
    {
        if (_physics == null)
            return;
        _physics.isKinematic = !enable;
    }

    public void Dead()
    {
        _health.SetDamageable(false);
        _fsm.Reset();

        EnablePhysics(false);
        ReturnToPoolAfter(3f);

        OnDead?.Invoke(this);
    }
    public override void OnSpawn()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Subscribe(OnBattleStateChanged);
            OnBattleStateChanged(BattleManager.Instance.State);
        }
    }

    public override void OnDespawn()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Unsubscribe(OnBattleStateChanged);
        }
        base.OnDespawn();
    }
    
    private void OnBattleStateChanged(EBattleState state)
    {
        switch (state)
        {
            case EBattleState.None:
                break;
            case EBattleState.Preparing:
                _wait = true;
                break;
            case EBattleState.InProgress:
                _wait = false;
                OnPause(false);
                break;
            case EBattleState.Pause:
                {
                    OnPause(true);
                    break;
                }
                
            case EBattleState.WaitingNextStage:
                _wait = true;
                break;
            case EBattleState.Victory:
                break;
            case EBattleState.Defeat:
                break;
        }
    }

    [Button]
    private void OnPause(bool paused)
    {
        _paused = paused;

        _move.SetPaused(paused);
        _anim.SetAnimSpeed(paused ? 0f : 1f);
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

    [Button]
    public void ApplyDamage(DamageData data)
    {
        if(_wait)
        {
            return;
        }

        _healthUI?.Show();
        if (!_health.TryApplyDamage(data.Damage))
        {
            return;
        }

        _phase?.CheckPhaseTransition();

        int dir = DirectionConvert(data.HitDirection);
        Anim.SetInt(EnemyAnimator.s_hitDirInt, dir);

        if (_health.IsDead)
        {
            HandleDead();
        }
        else
        {
            HandleDamaged();
        }
    }

    private int DirectionConvert( Vector3 hitDirection)
    {
        Vector3 localDir = transform.InverseTransformDirection(hitDirection);
        localDir.y = 0f;

        if (Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z))
        {
            return localDir.x < 0f? (int)EHitDirection.Right : (int)EHitDirection.Left;
        }

        return localDir.z < 0f ? (int)EHitDirection.Front : (int)EHitDirection.Back;
    }

    public void HandleDamaged()
    {
        if (FSM.CurrentState is DeadState || Stat.HasSuperArmor)
        {
            return;
        }

        _fsm.ChangeState(EEnemyState.Hit);
    }

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
        FSM.ChangeState(EEnemyState.Idle);
        _anim.SetTrigger(EnemyAnimator.s_resetTrigger);
    }

    public void OnBeginAttack()
    {
        _move.SetAbleToRatate(false);
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
        
        _attack.OnEndAttack();
    }
    public void OnAttackComplete()
    {
        StopLoopDelay();
        _move.SetAbleToRatate(true);
        Attack.Finish();

        if (FSM.CurrentState is AttackState attackState)
        {
            attackState.OnAttackFinished();
        }
    }

    #endregion
}

