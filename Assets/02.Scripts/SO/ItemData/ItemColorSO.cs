using UnityEngine;

[CreateAssetMenu(fileName = "ItemColorData", menuName = "Item/GradeColorData")]
public class ItemColorSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, Color> _colorDict;

    public Color GetColor(EItemGrade grade)
    {
        return _colorDict.GetValueOrDefault(grade);
    }
}
