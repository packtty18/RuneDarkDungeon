using UnityEngine;

public class ArcMovement : MonoBehaviour, IAttackMovement
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float gravity = -9.8f;

    public void Initialize(AttackObjectDataSO data)
    {
        // velocity can be injected here
    }

    public void Tick(float deltaTime)
    {
        velocity.y += gravity * deltaTime;
        transform.position += velocity * deltaTime;
    }
}
