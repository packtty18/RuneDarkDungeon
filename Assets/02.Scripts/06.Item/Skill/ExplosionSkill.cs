using UnityEngine;
using System.Collections;

public class ExplosionSkill : MonoBehaviour
{
    [SerializeField] private HitBox _explosion;
    [SerializeField] private float _lifeTime;
    private WaitForSeconds _wait;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _explosion = GetComponent<HitBox>();
    }
#endif

    private void Awake()
    {
        _wait = new(_lifeTime);
    }

    public void Explosion(float damage)
    {
        gameObject.SetActive(true);
        _explosion.Activate(damage);
        StartCoroutine(DeactivateRoutine());
    }
    
    private IEnumerator DeactivateRoutine()
    {
        yield return _wait;
        gameObject.SetActive(false);
    }
}
