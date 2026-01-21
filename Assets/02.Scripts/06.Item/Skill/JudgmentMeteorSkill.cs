using UnityEngine;

public class JudgmentMeteorSkill : SkillBase
{
    [Header("유니크 추가 스킬")]
    [SerializeField] private DotDealer _dotSkill;
    [SerializeField] private float _interval;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private HitBox _finalAttack;
    [SerializeField] private RangeData<float> _finalDamage;
    [SerializeField] private float _finalLifeTime;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _dotSkill = GetComponent<DotDealer>();
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
        var finalAttack = Instantiate(_finalAttack, _user.transform.position, Quaternion.identity);
        finalAttack.Activate(_finalDamage.GetRandomValue());
        Destroy(finalAttack, _finalLifeTime);
    }
}
