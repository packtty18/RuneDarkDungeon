using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(DotDealer))]
public class BladeStormSkill : SkillBase
{
    [Header("지속 데미지")]
    [SerializeField] private DotDealer _dotDealer;
    [SerializeField] private float _interval;
    
    [Header("유니크 범위 증가")]
    [SerializeField] private float _uniqueScale = 2f;
    [SerializeField] private float _scaleDuration = 0.5f;
    [SerializeField] private Ease _ease;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private float _pullForce = 1f;
    [SerializeField] private LayerMask _targetLayer;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _dotDealer = GetComponent<DotDealer>();
    }
#endif
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        transform.localScale = Vector3.one;
        transform.SetParent(user.transform);
        
        _dotDealer.StartDot(_damage[grade], _interval, _lifeTime);
        
        if (grade < EItemGrade.Unique) return;

        Vector3 targetScale = transform.localScale * _uniqueScale;

        transform.DOKill();
        transform.DOScale(targetScale, _scaleDuration).SetEase(_ease);
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetLayer) == 0) return;
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
