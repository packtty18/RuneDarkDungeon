using UnityEngine;

public interface IAttackMovement
{
    void Initialize(AttackObjectDataSO data);
    void Tick(float deltaTime);
}
