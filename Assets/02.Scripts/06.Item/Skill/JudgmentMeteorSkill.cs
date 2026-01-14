using UnityEngine;

public class JudgmentMeteorSkill : SkillBase
{
    [Header("레전드 추가 스킬")]
    [SerializeField] private GameObject _explosionPrefab;
    
    private void OnDestroy()
    {
        if (_grade != EItemGrade.Legendary) return;
        Instantiate(_explosionPrefab, _user.transform.position, Quaternion.identity);
    }
}
