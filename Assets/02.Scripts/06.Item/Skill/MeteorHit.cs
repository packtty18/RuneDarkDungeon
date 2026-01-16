using UnityEngine;

public class MeteorHit : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private RangeData<float> _damage;
    
    private void Awake()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform);
        
        if (explosion.TryGetComponent(out HitBox hitbox))
        {
            hitbox.Activate(_damage.GetRandomValue());
        }
    }
}
