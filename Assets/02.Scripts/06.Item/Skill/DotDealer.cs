using System.Collections;
using UnityEngine;

public class DotDealer : MonoBehaviour
{
    private float _damage;
    private float _interval;
    private float _duration;
    
    public void StartDot(float damage, float interval, float duration)
    {
        _damage = damage;
        _interval = interval;
        _duration = duration;
        StartCoroutine(TickDamageRoutine());
    }
    
    private IEnumerator TickDamageRoutine()
    {
        if (!TryGetComponent<HitBox>(out var hitbox)) yield break;
        float timer = 0;
        
        WaitForSeconds waitTick = new(_interval);
        
        while (timer < _duration)
        {
            hitbox.Activate(_damage);
            yield return waitTick;
            timer += _interval;
        }
    }
}
