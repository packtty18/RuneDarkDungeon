using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BladeStormSkill : SkillBase
{
    [Header("지속 피해 간격")]
    [SerializeField] private float _tick = 0.25f;
    
    [Header("유니크 범위 증가")]
    [SerializeField] private float _uniqueScale = 2f;

    [SerializeField] private float _scaleDuration = 0.5f;
    [SerializeField] private Ease _ease;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private float _pullForce = 1f;
    
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        transform.SetParent(user.transform);
        
        StartCoroutine(TickDamageRoutine());
        
        if (grade < EItemGrade.Unique) return;

        Vector3 targetScale = transform.localScale * _uniqueScale;

        transform.DOScale(targetScale, _scaleDuration) 
            .SetEase(_ease)
            .SetLink(gameObject);
    }

    private IEnumerator TickDamageRoutine()
    {
        if (!TryGetComponent<HitBox>(out var hitbox)) yield break;
        float timer = 0;
        
        WaitForSeconds waitTick = new(_tick);
        
        while (timer < _lifeTime)
        {
            hitbox.Activate(_damage);
            yield return waitTick;
            timer += _tick;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (_grade != EItemGrade.Legendary) return;
        PullEnemy(other.transform);
    }

    private void PullEnemy(Transform enemy)
    {
        Vector3 directionToPlayer = (_user.transform.position - enemy.position).normalized;

        directionToPlayer.y = 0;
        directionToPlayer.Normalize();

        enemy.position += directionToPlayer * _pullForce * Time.deltaTime;
    }
}
