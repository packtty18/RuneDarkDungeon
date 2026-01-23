using System.Collections;
using UnityEngine;

public class DotDealer : MonoBehaviour
{
    private RangeData<float> _damage;
    private float _interval;
    private float _duration;
    
    public void StartDot(RangeData<float> damage, float interval, float duration)
    {
        gameObject.SetActive(true);
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
            hitbox.Activate(_damage.GetRandomValue());
            yield return waitTick;
            timer += _interval;
        }

        gameObject.SetActive(false);
    }
}
