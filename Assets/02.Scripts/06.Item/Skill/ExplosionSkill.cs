using UnityEngine;

public class ExplosionSkill : MonoBehaviour
{
    [SerializeField] private HitBox _explosion;
    [SerializeField] private float _lifeTime;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _explosion = GetComponent<HitBox>();
    }
#endif
    
    public void Explosion(float damage)
    {
        _explosion.Activate(damage);
    }
    
    
}
