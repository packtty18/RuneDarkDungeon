using UnityEngine;

[CreateAssetMenu(fileName = "GradeColorData", menuName = "Item/GradeColorData")]
public class GradeColorSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, Color> _colorDict;

    public Color GetColor(EItemGrade grade)
    {
        return _colorDict.GetValueOrDefault(grade);
    }
}
