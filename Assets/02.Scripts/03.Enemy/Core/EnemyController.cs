using Sirenix.OdinInspector;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// 전체적인 조작을 담당.
// State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : PoolableObject, IDamageable
{
    #region Fields & Properties

    [SerializeField] private ETeamType _team;
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Collider _physicCollider;
    [SerializeField] private Transform _target;
    

    [ShowInInspector] private IEnemyBehavior _behavior;
    [ShowInInspector] private EnemyStateMachine _fsm;

    [Title("Components")]
    [SerializeField] private EnemyPhase _phase;
    [SerializeField] private EnemyMove _move;
    [SerializeField] private EnemyAttack _attack;
    [SerializeField] private EnemyHealth _health;
    [SerializeField] private EnemyStat _stat;
    [SerializeField] private EnemyAnimator _anim;
    [SerializeField] private EnemyBuff _buff;
    [SerializeField] private EnemySound _sound;
    [SerializeField] private EnemyShaderFeedback _shader;
    [SerializeField] private ItemDropper _dropper;

    [Title("boolean")]
    [SerializeField] private bool _paused;
    [SerializeField] private bool _wait = false;

    [SerializeField] private bool _canDropItem = false;

    [Title("optional")]
    [SerializeField] private EnemyHealthUI _healthUI;
    [SerializeField] private TrailRenderer _weaponTrail;

    public void SetItemDrop(bool enabled)
    {
        _canDropItem = enabled;
    }


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

    #endregion

    #region Unity Lifecycle

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
        _dropper = GetComponent<ItemDropper>();

        EnablePhysics(true);
        _fsm = new EnemyStateMachine(this);
        OnTrailEnd();
    }

    private void Update()
    {
        if (!CanTick())
        {
            return;
        }

        _fsm.Tick(Time.deltaTime);
    }

    #endregion

    #region Pool Lifecycle

    public override void OnSpawn()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStateChanged.Subscribe(OnBattleStateChanged);
            OnBattleStateChanged(BattleManager.Instance.State);
        }

        _target = null;

        EnablePhysics(false);
        OnTrailEnd();
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

        if (_move != null)
        {
            _move.ResetAgent();
        }

        StopAllCoroutines();
        loopRoutine = null;

        UIDisable();
        EnablePhysics(false);
  
        base.OnDespawn();
    }

    #endregion

    #region Initialization

    [Button]
    public void Init()
    {
        _stat.Init();

        _health.Init();
        _move.Init();
        _attack.Init();
        _anim.Init();
        _buff.Init();
        _sound.Init();
        _shader.Init();

        _behavior = CreateBehavior(_stat.EnemyType);
        _behavior?.Initialize(this);
        _phase?.Reset();

        UIEnable();

        _fsm.Reset();
        _fsm.ChangeState(EEnemyState.Idle);

        EnablePhysics(true);

        if (_stat.EnemyType == EEnemyType.Boss)
        {
            SetActiveSuperArmor(true);
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

    #endregion

    #region Tick Control

    private bool CanTick()
    {
        if (_paused)
            return false;

        if (FSM.CurrentState is DeadState)
            return false;

        return true;
    }

    [Button]
    private void OnPause(bool paused)
    {
        _paused = paused;

        _move.SetPaused(paused);
        _anim.SetAnimSpeed(paused ? 0f : 1f);
    }

    #endregion

    #region Battle State

    private void OnBattleStateChanged(EBattleState state)
    {
        switch (state)
        {
            case EBattleState.Preparing:
            case EBattleState.WaitingNextStage:
                _wait = true;
                break;

            case EBattleState.InProgress:
                _wait = false;
                OnPause(false);

                if (Stat.EnemyType == EEnemyType.Boss)
                {
                    UIEnable();
                }
                break;

            case EBattleState.Pause:
                OnPause(true);
                break;
        }
    }

    #endregion

    #region Physics Control

    public void EnablePhysics(bool enable)
    {
        if (_physicCollider == null)
            return;

        _physicCollider.isTrigger = !enable;
    }

    #endregion

    #region Target

    public void SetTarget(Transform target)
    {
        if (target == null)
            return;

        _target = target;
    }

    public bool IsTargetExist()
    {
        return _target != null;
    }

    public Vector3 GetTargetPosition()
    {
        return _target != null ? _target.position : transform.position;
    }

    public bool IsTargetInRange(float range)
    {
        if (_target == null)
            return false;

        float sqrDistance = (transform.position - _target.position).sqrMagnitude;
        return sqrDistance <= range * range;
    }

    #endregion

    #region Shader Feedback

    public void SetActiveSuperArmor(bool enable)
    {
        if (enable)
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

    public void PlayDeadFade()
    {
        Shader.PlayDeathFade();
    }

    #endregion

    #region Health & Damage

    [Button]
    public void ApplyDamage(DamageData data)
    {
        if (_wait)
            return;

        Shader.PlayHit();

        if (!_health.TryApplyDamage(data.Damage))
            return;

        _phase?.CheckPhaseTransition();

        int dir = DirectionConvert(data.HitDirection);
        Anim.SetInt(EnemyAnimator.s_hitDirInt, dir);

        if (_health.IsDead)
            HandleDead();
        else
            HandleDamaged();
    }

    private int DirectionConvert(Vector3 hitDirection)
    {
        Vector3 localDir = transform.InverseTransformDirection(hitDirection);
        localDir.y = 0f;

        if (Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z))
        {
            return localDir.x < 0f ? (int)EHitDirection.Right : (int)EHitDirection.Left;
        }

        return localDir.z < 0f ? (int)EHitDirection.Front : (int)EHitDirection.Back;
    }

    public void HandleDamaged()
    {
        if (FSM.CurrentState is DeadState || Stat.HasSuperArmor)
            return;

        _fsm.ChangeState(EEnemyState.Hit);
    }

    public void HandleDead()
    {
        if (FSM.CurrentState is DeadState || !gameObject.activeSelf)
            return;

        _fsm.ChangeState(EEnemyState.Dead);
    }

    public void Dead()
    {
        EnablePhysics(false);
        UIDisable();
        ReturnToPoolAfter(5);
        
        OnDead?.Invoke(this);
    }

    public void DropItem()
    {
        if(!_canDropItem || _dropper == null)
        {
            return;
        }

        _dropper.Drop();
    }

    #endregion

    #region Animation Events

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

    public void OnTrailStart()
    {
        if (_weaponTrail == null)
            return;

        _weaponTrail.emitting = true;
    }

    public void OnTrailEnd()
    {
        if (_weaponTrail == null)
            return;

        _weaponTrail.emitting = false;
    }

    #endregion

    #region Attack Loop Helper

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

    private void PlaySlashSound()
    {
        if (loopRoutine != null)
        {
            StopCoroutine(loopRoutine);
            loopRoutine = null;
        }
    }


    #endregion

    #region UI

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

    #endregion

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (FSM.CurrentState is ChargeState)
        {
            int layer = collision.gameObject.layer;

            if (layer == LayerMask.NameToLayer("Wall") ||
                layer == LayerMask.NameToLayer("Obstacle") ||
                layer == LayerMask.NameToLayer("Default"))
            {
                Debug.Log($"[EnemyController] 충돌 감지: {collision.gameObject.name} - 돌진 중단");
                FSM.ChangeState(EEnemyState.Chase);
            }
        }
    }

    #endregion
}
