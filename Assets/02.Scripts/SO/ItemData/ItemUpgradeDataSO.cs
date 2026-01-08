using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Item/UpgradeData")]
public class ItemUpgradeDataSO : ScriptableObject
{
    [SerializeField] private SerializableDictionary<EItemGrade, UpgradeData> _gradeDict;
    
    public UpgradeData? GetGradeInfo(EItemGrade grade)
    {
        if (_gradeDict.TryGetValue(grade, out UpgradeData data))
        {
            return data;
        }
        return null;
    }
}
