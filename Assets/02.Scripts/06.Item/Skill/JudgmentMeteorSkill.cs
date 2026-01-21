using UnityEngine;

public class JudgmentMeteorSkill : SkillBase
{
    [Header("유니크 추가 스킬")]
    [SerializeField] private DotDealer _skillPrefab;
    [SerializeField] private float _interval;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private GameObject _finalEffectPrefab;
    [SerializeField] private RangeData<float> _finalDamage;
    [SerializeField] private float _finalLifeTime;
    
#if UNITY_EDITOR
    private void Reset()
    {
        _skillPrefab = GetComponent<DotDealer>();
    }
#endif
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        SoundManager.Instance.Play(ESoundType.Skill_JudgmentMeteor,user.transform.position);
        
        if (_grade < EItemGrade.Unique) return;
        var dotSkill = Instantiate(_skillPrefab, transform);
        dotSkill.StartDot(_damage[grade], _interval, _lifeTime);
    }

    private void OnDestroy()
    {
        if (_grade != EItemGrade.Legendary) return;
        var finalEffect = Instantiate(_finalEffectPrefab, _user.transform.position, Quaternion.identity);
        if (!finalEffect.TryGetComponent<HitBox>(out var hitbox)) return;
        hitbox.Activate(_finalDamage.GetRandomValue());
        Destroy(finalEffect, _finalLifeTime);
    }
}
