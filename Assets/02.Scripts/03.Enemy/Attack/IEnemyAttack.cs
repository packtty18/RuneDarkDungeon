using System;
using UnityEngine;

public interface IEnemyAttack
{
    string Name { get; }    
    EAttackType AttackType { get; }
    bool CanExecute { get; }
    SafeEvent OnAttackFinished { get; }
    void Execute();
    void Cancel();
}