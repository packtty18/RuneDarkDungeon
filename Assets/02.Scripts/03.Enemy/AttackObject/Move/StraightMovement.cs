using UnityEngine;


public class StraightMovement : MonoBehaviour, IAttackMovement
{
    [SerializeField] private float speed = 10f;

    public void Initialize(AttackObjectDataSO data)
    {
        // Optional: override speed from data
    }

    public void Tick(float deltaTime)
    {
        transform.position += transform.forward * speed * deltaTime;
    }
}
