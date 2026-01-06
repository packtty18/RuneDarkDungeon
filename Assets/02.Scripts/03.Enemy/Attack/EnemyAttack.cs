using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public enum EAttackType
{
    Any = 0,
    Melee,
    Ranged,
    Buff
}

public class EnemyAttack : SerializedMonoBehaviour
{
    [ShowInInspector]
    private readonly Dictionary<EAttackType, List<IEnemyAttack>> _attackMap = new();

    private readonly List<IEnemyAttack> _cachedExecutableAttacks = new();
    private IEnemyAttack _currentAttack;

    [SerializeField] private bool _onTest;


    [Button, ShowIf(nameof(_onTest))]
    public void RegisterAttacks()
    {
        _attackMap.Clear();

        var attacks = GetComponentsInChildren<IEnemyAttack>();

        foreach (var attack in attacks)
        {
            if (!_attackMap.TryGetValue(attack.AttackType, out var list))
            {
                list = new List<IEnemyAttack>();
                _attackMap.Add(attack.AttackType, list);
            }

            list.Add(attack);
        }

        Debug.Log($"[EnemyAttack] {_attackMap.Count}개의 공격 등록", this);
    }

    [Button, ShowIf(nameof(_onTest))]
    public void ExecuteAttack(EAttackType type = EAttackType.Any)
    {
        CollectExecutableAttacks(type, _cachedExecutableAttacks);

        if (_cachedExecutableAttacks.Count == 0)
        {
            Debug.Log($"[EnemyAttack] 현재 사용 가능한 공격이 없음 ({type})", this);
            return;
        }

        ExecuteSelectedAttack(_cachedExecutableAttacks[Random.Range(0, _cachedExecutableAttacks.Count)],type);
    }

    [Button, ShowIf(nameof(_onTest))]
    public void CancelAttack()
    {
        if (_currentAttack == null)
        {
            Debug.Log("[EnemyAttack] 실행중인 공격이 없음.", this);
            return;
        }

        _currentAttack.Cancel();
        ResetAttack();
    }


    private void CollectExecutableAttacks(EAttackType type, List<IEnemyAttack> result)
    {
        result.Clear();

        // Any : 모든 타입 수집
        if (type == EAttackType.Any)
        {
            foreach (var pair in _attackMap)
            {
                CollectFromList(pair.Value, result);
            }
        }
        // 특정 타입 수집
        else
        {
            if (_attackMap.TryGetValue(type, out var list))
            {
                CollectFromList(list, result);
            }
        }
    }

    private void CollectFromList(List<IEnemyAttack> source,List<IEnemyAttack> result)
    {
        foreach (var attack in source)
        {
            if (attack.CanExecute)
            {
                result.Add(attack);
            }
        }
    }

    private void ExecuteSelectedAttack(IEnemyAttack attack,EAttackType type)
    {
        // 기존 공격 정리
        if (_currentAttack != null)
        {
            _currentAttack.Cancel();
            _currentAttack.OnAttackFinished.Unsubscribe(ResetAttack);
        }

        _currentAttack = attack;
        _currentAttack.OnAttackFinished.Subscribe(ResetAttack);
        _currentAttack.Execute();

        Debug.Log($"[EnemyAttack] {_currentAttack.Name} 실행 ({type})", this);
    }

    private void ResetAttack()
    {
        if (_currentAttack == null)
        {
            return;
        }

        _currentAttack.OnAttackFinished.Unsubscribe(ResetAttack);
        _currentAttack = null;
    }
}

