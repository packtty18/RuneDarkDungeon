using UnityEngine;

[RequireComponent(typeof(DotDealer))]
public class LightningZoneSkill : SkillBase
{
    [Header("지속 데미지")]
    [SerializeField] private DotDealer _dotDealer;
    [SerializeField] private float _interval;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private HitBox _finalAttack;
    [SerializeField] private RangeData<float> _finalDamage;
    
    [Header("이펙트 오프셋")]
    [SerializeField] private Vector3 _offset;

#if UNITY_EDITOR
    private void Reset()
    {
        _dotDealer = GetComponent<DotDealer>();
    }
#endif
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        transform.position += _offset;
        
        _dotDealer.StartDot(_damage[grade], _interval, _lifeTime);
        
        if (grade < EItemGrade.Unique) return;
        transform.SetParent(user.transform);
    }

    public override void OnDespawn()
    {
        if (_grade != EItemGrade.Legendary) return;
        var finalAttack = Instantiate(_finalAttack, _user.transform.position, Quaternion.identity);
        finalAttack.Activate(_finalDamage.GetRandomValue());
    }
}
