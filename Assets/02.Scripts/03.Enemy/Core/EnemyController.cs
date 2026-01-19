using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;


//전체적인 조작을 담당.
//State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField] private ETeamType _team;
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Collider _physicCollider;
    [SerializeField] private Transform _target;
    [SerializeField] private EnemyHealthUI _healthUI;

    [ShowInInspector] private IEnemyBehavior _behavior;
    [ShowInInspector] private EnemyStateMachine _fsm;

    [SerializeField] private EnemyPhase _phase;
    [SerializeField] private EnemyMove _move;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private EnemyAnimator _anim;
    [SerializeField] private EnemyBuff _buff;
    [SerializeField] private EnemySound _sound;
    [SerializeField] private EnemyShaderFeedback _shader;

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
    public EnemySound Sound => _sound;
    public EnemyShaderFeedback Shader => _shader;
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
        _buff = GetComponent<EnemyBuff>();
        _sound = GetComponent<EnemySound>();
        _phase = GetComponent<EnemyPhase>();
        _shader = GetComponent<EnemyShaderFeedback>();

        _rigid = GetComponent<Rigidbody>();
        _physicCollider = GetComponent<Collider>();

        EnablePhysics(true);
        _fsm = new EnemyStateMachine(this);
    }



    #region 생명주기
    public override void OnSpawn()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Subscribe(OnBattleStateChanged);
            OnBattleStateChanged(BattleManager.Instance.State);
        }
        _target = null;

        // 물리 엔진 비활성화 (위치 설정 전)
        EnablePhysics(false);

        // NavMeshAgent도 비활성화
        if (_move != null)
        {
            _move.ResetAgent();
        }
    }

    public override void OnDespawn()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Unsubscribe(OnBattleStateChanged);
        }

        _fsm.Reset();
        StopAllCoroutines();
        loopRoutine = null;
        UIDisable();
        EnablePhysics(false);
        _move.ResetAgent();

        base.OnDespawn();
    }

    [Button]
    public void Init()
    {
        // 1. Stat 먼저 초기화 (다른 시스템에서 참조하므로)
        _stat.Init();

        // 2. 나머지 컴포넌트 초기화 (Agent는 비활성화 상태 유지)
        _health.Init();
        _move.Init(); // Agent 비활성화
        _attack.Init();
        _anim.Init();
        _buff.Init();
        _sound.Init();
        _shader.Init();

        _behavior = CreateBehavior(_stat.EnemyType);
        _behavior?.Initialize(this);
        _phase?.Reset();

        UIEnable();


        // 5. FSM 초기화 및 시작 (Idle 상태에서 Agent 활성화됨)
        _fsm.Reset();
        _fsm.ChangeState(EEnemyState.Idle);

        // 6. 물리 엔진 활성화 (마지막에 수행)
        EnablePhysics(true);

        if (_stat.EnemyType == EEnemyType.Boss)
        {
            SetActiveSuperArmor(true);
        }
    }

    private void UIEnable()
    {
        if (_healthUI != null)
        {
            _healthUI.Init();
        }
        if (_wait && Stat.EnemyType == EEnemyType.Boss)
        {
            _healthUI.Hide();
        }
    }

    private void UIDisable()
    {
        if (_healthUI != null)
        {
            _healthUI.Hide();
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


    public void SetConstraintsPosition(bool enable)
    {
        if (_rigid == null)
            return;

        RigidbodyConstraints constraints = _rigid.constraints;
        if (enable)
        {
            constraints |= RigidbodyConstraints.FreezePositionX;
            constraints |= RigidbodyConstraints.FreezePositionY;
            constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            constraints &= ~RigidbodyConstraints.FreezePositionX;
            constraints &= ~RigidbodyConstraints.FreezePositionY;
            constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }

        _rigid.constraints = constraints;
    }

    public void EnablePhysics(bool enable)
    {
        if (_rigid == null)
            return;
        _rigid.isKinematic = !enable;
        _physicCollider.isTrigger = !enable;
    }

    public void Dead()
    {
        EnablePhysics(false);
        UIDisable();
        ReturnToPoolAfter(5);
        OnDead?.Invoke(this);
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
                if(Stat.EnemyType == EEnemyType.Boss)
                {
                    UIEnable();
                }
                break;
            case EBattleState.Pause:
                OnPause(true);
                break;
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
   
    #region Shader가 적용되는 기능
    public void SetActiveSuperArmor(bool enable)
    {
        if(enable)
        {
            Shader.EnableSuperArmorOutline();
            Stat.EnableSuperArmor();
        }
        else
        {
            Shader.DisableSuperArmorOutline();
            Stat.DisableSuperArmor();
        }
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

        Shader.PlayHit();

        // 데미지 적용 (EnemyHealthUI가 자동으로 표시됨)
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
    public void PlayDeadFade()
    {
        Shader.PlayDeathFade();
    }

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


    private void OnCollisionEnter(Collision collision)
    {
        // 돌진 중일 때만 처리
        if (FSM.CurrentState is ChargeState chargeState)
        {
            // 벽이나 장애물과 충돌 시 돌진 중단
            if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                Debug.Log($"[EnemyController] 충돌 감지: {collision.gameObject.name} - 돌진 중단");
                FSM.ChangeState(EEnemyState.Chase);
            }
        }
    }
}

