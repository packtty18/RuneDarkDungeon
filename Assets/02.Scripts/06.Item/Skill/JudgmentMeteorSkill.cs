using UnityEngine;

public class JudgmentMeteorSkill : SkillBase
{
    [Header("유니크 추가 스킬")]
    [SerializeField] private GameObject _skillPrefab;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private GameObject _finalEffectPrefab;

    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        if (_grade < EItemGrade.Unique) return;
        Instantiate(_skillPrefab, transform);
    }

    private void OnDestroy()
    {
        if (_grade != EItemGrade.Legendary) return;
        var finalEffect = Instantiate(_finalEffectPrefab, _user.transform.position, Quaternion.identity);
        if (!finalEffect.TryGetComponent<HitBox>(out var hitbox)) return;
        hitbox.Activate(_damage);    }
}
