using System.Collections;
using UnityEngine;

public class MagmaArea : MonoBehaviour
{
    [Header("데미지 설정")]
    [SerializeField] private float _tick;
    [SerializeField] private float _damage;
    
    public void Awake()
    {
        StartCoroutine(TickDamageRoutine());
    }
    
    private IEnumerator TickDamageRoutine()
    {
        if (!TryGetComponent<HitBox>(out var hitbox)) yield break;
        
        WaitForSeconds waitTick = new(_tick);
        
        hitbox.Activate(_damage);
        yield return waitTick;
    }
}
