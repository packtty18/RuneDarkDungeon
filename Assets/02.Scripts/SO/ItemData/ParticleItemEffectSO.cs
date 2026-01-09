using UnityEngine;

[CreateAssetMenu(fileName = "ParticleItemEffect_", menuName = "Item/ItemEffect/ParticleItemEffect")]
public class ParticleItemEffectSO : ItemEffectBaseSO
{
    [Header("이펙트 프리팹 연결")]
    [SerializeField] private SerializableDictionary<EItemGrade, ParticleEffect> _effectDict;

    [Header("사용자 위치를 추적")]
    [SerializeField] private bool _isFollowUser;
    
    private ParticleEffect GetEffect(EItemGrade grade)
    {
        return _effectDict.GetValueOrDefault(grade);
    }
    
    public override void OnUse(GameObject user, EItemGrade grade)
    {
        var effect = GetEffect(grade);

        if (_isFollowUser)
        {
            Instantiate(effect, user.transform);
        }
        else
        {
            Instantiate(effect, user.transform.position, Quaternion.identity);
        }
    }
}
