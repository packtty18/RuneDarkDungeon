using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

//전체적인 조작을 담당.
//State머신에서는 여기에 있는 함수만을 사용함.
public class EnemyController : MonoBehaviour
{
    public EnemyFacade Facade { get; private set; }

    [SerializeField] private Collider _physicsCollider;

    [SerializeField] private EnemyStateMachine _fsm;
    [SerializeField] private Transform _target;


    public EnemyStateMachine FSM => _fsm;
    private EnemyMove _move => Facade.Move;
    private EnemyAttack _attack => Facade.Attack;
    private EnemyHealth _health => Facade.Health;
    private EnemyStat _stat => Facade.Stat;
    private AnimatorController _anim => Facade.Anim;

    private void Awake()
    {
        Facade = GetComponent<EnemyFacade>();
        _physicsCollider = GetComponent<Collider>();
        _fsm = new EnemyStateMachine();
        _fsm.ChangeState(new IdleState(this));
    }

    private void Update()
    {
        _fsm.Update();
    }

    public void ChangeState(EnemyState state)
    {
        _fsm.ChangeState(state);
    }

    #region 생명주기관련
    public void Init()
    {
        _physicsCollider.enabled = true;
        Facade.Init();
    }
    public void Dead()
    {
        _move.PauseAgent();
        _attack.CancelAttack();
        _anim.SetTrigger(AnimatorController.s_trigger_Dead);

        // 물리 Collider 비활성화
        _physicsCollider.enabled = false;

        StartCoroutine(Util.DestroyAfterTime(3f, gameObject));
    }
    #endregion

    #region Target
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
    #endregion

    #region Move관련
    public void MoveToTarget()
    {
        _move.SetTarget(_target);
        _move.StartMove();
        _anim.SetBool(AnimatorController.s_bool_IsMove, true);
    }

    public void StopMove()
    {
        _move.StopMove();
        _anim.SetBool(AnimatorController.s_bool_IsMove, false);
    }
    #endregion

    #region Attack관련
    public bool IsOnAttack()
    {
        return _attack.IsAttacking;
    }

    public int GetAttackCount()
    {
        return _attack.StrategyCount;
    }

    public void RequestAttack(int attackID)
    {
        if(!_attack.RequestAttack(attackID))
        {
            return;
        }

        _anim.SetInt(AnimatorController.s_int_AttackID, attackID);
        _anim.SetTrigger(AnimatorController.s_trigger_Attack);
    }
    #endregion
    

    public void EnemyHitted()
    {
        _move.PauseAgent();
        _attack.CancelAttack();
        _anim.SetTrigger(AnimatorController.s_trigger_Hit);
    }


    #region 애니메이션 이벤트용
    public void HitRecover()
    {
        FSM.ChangeState(new IdleState(this));
        _anim.SetTrigger(AnimatorController.s_trigger_Reset);
    }

    public void OnBeginAttack()
    {
        _attack.OnBeginAttack();
    }

    public void OnLoopEnd()
    {
        _attack.OnLoopEnd();
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

