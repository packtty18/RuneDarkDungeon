using UnityEngine;

[CreateAssetMenu(fileName = "ItemBorderData", menuName = "Item/ItemBorderData")]
public class ItemBorderSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, Sprite> _borderDict;

    public Sprite GetBorderSprite(EItemGrade grade)
    {
        return _borderDict.GetValueOrDefault(grade);
    }
}
