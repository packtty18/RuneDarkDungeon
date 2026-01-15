using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * 오로지 공격의 실행
 * 공격실행의 판단은 FSM에서
 */
public class EnemyAttack : MonoBehaviour
{
    [Title("Reference")]
    [SerializeField] protected EnemyController controller;
    [SerializeField] protected HitboxController _hitboxController;

    [ShowInInspector] private readonly Dictionary<int, IAttackStretagy> MeleeAttack = new();
    [ShowInInspector] private IAttackStretagy current;

    protected float _damage =>controller.Stat.GetValue(EEnemyValueFloat.Attack).Value;
    public float CurrentLoopDelay => current == null ? 0f : current.LoopDelay;


    public virtual void Init()
    {
        if (controller == null)
        {
            controller = GetComponent<EnemyController>();
        }
        _hitboxController = GetComponentInChildren<HitboxController>();
        MeleeAttack.Clear();
        current = null;

    }

    #region Strategy
    protected void RegisterStrategy(int id, IAttackStretagy strategy)
    {
        if (MeleeAttack.ContainsKey(id))
        {
            Debug.LogWarning($"[EnemyAttack] Strategy already registered: {id}");
            return;
        }

        MeleeAttack.Add(id, strategy);
        Debug.Log($"[EnemyAttack] Strategy registered: {id}");
    }

    #endregion

    #region Execute
    public bool Execute(int attackId)
    {
        if (!MeleeAttack.TryGetValue(attackId, out var strategy))
        {
            Debug.LogWarning($"[EnemyAttack] No strategy for ID {attackId}");
            return false;
        }

        current = strategy;
        Debug.Log($"[EnemyAttack] Execute Attack {attackId}");
        return true;
    }

    public void Finish()
    {
        current = null;
    }
    #endregion

    #region 엘리트,보스 - 돌진
    public virtual void StartCharge()
    {
        controller.Stat.SetActiveCharge(false);
        _hitboxController.Activate("Charge", _damage);
    }

    public virtual void EndCharge()
    {
        StartCoroutine(ChargeDelay());
        _hitboxController.Deactivate("Charge");
    }

    private IEnumerator ChargeDelay()
    {
        yield return new WaitForSeconds(controller.Stat.GetValue(EEnemyValueFloat.ChargeCooldown).Value);

        controller.Stat.SetActiveCharge(true);
        Debug.Log($"[{this}] : 돌진 충전 완료");
    }
    #endregion

    #region 보스 - 소환
    public virtual void StartSummon()
    {
        controller.Stat.SetActiveSummon(false);
    }

    public virtual void EndSummon()
    {
        StartCoroutine(ChargeSummon());
    }

    private IEnumerator ChargeSummon()
    {
        yield return new WaitForSeconds(30);

        controller.Stat.SetActiveSummon(true);
        Debug.Log($"[{this}] : 소환 충전 완료");
    }
    #endregion

    #region 보스 - 버프
    public virtual void StartBuff()
    {
        controller.Stat.SetActiveBuff(false);
    }

    public virtual void EndBuff()
    {
        StartCoroutine(ChargeBuff());
    }

    private IEnumerator ChargeBuff()
    {
        yield return new WaitForSeconds(40);

        controller.Stat.SetActiveBuff(true);
        Debug.Log($"[{this}] : 버프 충전 완료");
    }
    #endregion

    #region Animation Events
    public void OnBeginAttack()
    {
        Debug.Log($"[Attack] BeginAttack frame:{Time.frameCount}");
        current?.BeginAttack();
    }

    public void OnEndAttack()
    {
        Debug.Log($"[Attack] EndAttack frame:{Time.frameCount}");
        current?.EndAttack();
    }

    public int GetRandomAttackId()
    {
        List<int> list = new List<int>();

        foreach (int id in MeleeAttack.Keys)
        {
            list.Add(id);
        }

        if (list.Count == 1)
        {
            return list[0];
        }

        int random = Random.Range(0, list.Count);
        return list[random];
    }
    #endregion
}
