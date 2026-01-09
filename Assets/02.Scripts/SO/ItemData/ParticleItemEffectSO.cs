using UnityEngine;

[CreateAssetMenu(fileName = "ParticleItemEffect_", menuName = "Item/ItemEffect/ParticleItemEffect")]
public class ParticleItemEffectSO : ItemEffectBaseSO
{
    [Header("이펙트 프리팹 연결")]
    [SerializeField] private SerializableDictionary<EItemGrade, GameObject> _effectDict;

    private GameObject GetEffect(EItemGrade grade)
    {
        return _effectDict.GetValueOrDefault(grade);
    }
    
    public override void OnUse(GameObject user, EItemGrade grade)
    {
        var effect = GetEffect(grade);
        effect.SetActive(false);
        effect.SetActive(true);
        effect.transform.position = user.transform.position;
    }
}
