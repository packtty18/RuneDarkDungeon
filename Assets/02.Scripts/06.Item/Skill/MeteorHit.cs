using UnityEngine;

public class MeteorHit : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;

    private void Awake()
    {
        Instantiate(_explosionPrefab, transform);
    }
}
