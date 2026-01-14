using UnityEngine;

public class BladeStormSkill : SkillBase
{
    [Header("유니크 범위 증가")]
    [SerializeField] private float _uniqueScale = 2f;
    
    [Header("레전드 추가 스킬")]
    [SerializeField] private float _pullForce = 10f;
    
    protected override void ApplyEffect(GameObject user, EItemGrade grade)
    {
        if(grade >= EItemGrade.Unique)
        {
            
        }
    }
}
