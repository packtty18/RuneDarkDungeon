using UnityEngine;

[RequireComponent(typeof(DotDealer))]
public class LightningZoneSkill : SkillBase
{
    [Header("지속 데미지")]
    [SerializeField] private DotDealer _dotDealer;
    [SerializeField] private float _interval;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private GameObject _finalEffectPrefab;
    
    [Header("이펙트 오프셋")]
    [SerializeField] private Vector3 _offset;

#if UNITY_EDITOR
    void Reset()
    {
        _dotDealer = GetComponent<DotDealer>();
    }
#endif
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        transform.position += _offset;
        
        _dotDealer.StartDot(_damage, _interval, _lifeTime);
        
        if (grade < EItemGrade.Unique) return;
        transform.SetParent(user.transform);
    }

    private void OnDestroy()
    {
        if (_grade != EItemGrade.Legendary) return;
        var finalEffect = Instantiate(_finalEffectPrefab, _user.transform.position, Quaternion.identity);
        if (!finalEffect.TryGetComponent<HitBox>(out var hitbox)) return;
        hitbox.Activate(_damage);
    }
}
