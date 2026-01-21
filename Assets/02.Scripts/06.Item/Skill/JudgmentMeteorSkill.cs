using UnityEngine;

public class JudgmentMeteorSkill : SkillBase
{
    [Header("유니크 추가 스킬")]
    [SerializeField] private DotDealer _dotSkill;
    [SerializeField] private float _interval;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private FinalAttack _finalAttack;
    [SerializeField] private RangeData<float> _finalDamage;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _dotSkill = GetComponent<DotDealer>();
        _finalAttack = GetComponent<FinalAttack>();
    }
#endif
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        if (_grade < EItemGrade.Unique) return;
        _dotSkill.StartDot(_damage[grade], _interval, _lifeTime);
    }

    public override void OnDespawn()
    {
        if (_grade != EItemGrade.Legendary) return;
        _finalAttack.Spawn(_finalDamage.GetRandomValue());
    }
}
