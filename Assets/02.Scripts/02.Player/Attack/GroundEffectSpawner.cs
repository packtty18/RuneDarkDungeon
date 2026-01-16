using UnityEngine;

public class GroundEffectSpawner : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Raycast Settings")]
    [SerializeField] private float rayDistance = 15;
    private Vector3 _rayOffset = Vector3.up * 5f;

    public void SpawnGroundEffect(EffectPlayer effect, Vector3 originPosition, Transform parent, float Damage = 0)
    {
        Vector3 rayStart = originPosition + _rayOffset;
        Ray ray = new Ray(rayStart, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, groundLayer))
        {
            // 바닥 위치에 이펙트 생성
            Vector3 spawnPosition = hit.point;

            effect.PlayDealEffectWorldPosition(parent, spawnPosition, Damage);

            // 디버그 라인
            Debug.DrawLine(rayStart, hit.point, Color.green, 2f);
        }
        else
        {
            Debug.LogWarning("Ground not found!");
            Debug.DrawRay(rayStart, Vector3.down * rayDistance, Color.red, 2f);
        }
    }
}