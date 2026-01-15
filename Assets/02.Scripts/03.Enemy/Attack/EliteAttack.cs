using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;

public class EliteAttack : EnemyAttack
{
    [SerializeField] protected HitboxController _hitboxController;
    
    public override void Init()
    {
        base.Init();


        RegisterStrategy(0, new EnemyDirectAttack(_hitboxController, "Main", _damage));   //기본공격1
    }

    public virtual void StartCharge()
    {
        controller.Stat.CanCharge = false;
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

        controller.Stat.CanCharge  = true;
        Debug.Log($"[{this}] : 돌진 충전 완료");
    }
}
