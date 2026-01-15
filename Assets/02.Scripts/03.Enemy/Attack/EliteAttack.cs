using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EliteAttack : EnemyAttack
{
    public override void Init()
    {
        base.Init();


        RegisterStrategy(0, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //기본공격1
    }
}
