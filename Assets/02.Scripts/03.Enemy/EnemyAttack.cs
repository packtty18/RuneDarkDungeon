using Sirenix.OdinInspector;
using System;
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

    [ShowInInspector] private readonly Dictionary<int, IAttackStretagy> strategies = new();
    [ShowInInspector] private IAttackStretagy current;

    protected float _damage =>controller.Stat.GetValue(EEnemyValueFloat.Attack).Value;
    public float CurrentLoopDelay => current == null ? 0f : current.LoopDelay;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    public virtual void Init()
    {
        strategies.Clear();
        current = null;
    }

    #region Strategy
    protected void RegisterStrategy(int id, IAttackStretagy strategy)
    {
        if (strategies.ContainsKey(id))
        {
            Debug.LogWarning($"[EnemyAttack] Strategy already registered: {id}");
            return;
        }

        strategies.Add(id, strategy);
        Debug.Log($"[EnemyAttack] Strategy registered: {id}");
    }
    #endregion

    #region Execute
    public bool Execute(int attackId)
    {
        if (!strategies.TryGetValue(attackId, out var strategy))
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

    #region Animation Events
    public void OnBeginAttack()
    {
        current?.BeginAttack();
    }

    public void OnEndAttack()
    {
        current?.EndAttack();
    }

    public int GetRandomAttackId()
    {
        List<int> list = new List<int>();
        foreach(int id in strategies.Keys)
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
