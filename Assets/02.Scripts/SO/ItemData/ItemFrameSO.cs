using UnityEngine;

[CreateAssetMenu(fileName = "ItemFrameData", menuName = "Item/ItemFrameData")]
public class ItemFrameSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, Sprite> _frameDict;

    public Sprite GetBorderSprite(EItemGrade grade)
    {
        return _frameDict.GetValueOrDefault(grade);
    }
}
