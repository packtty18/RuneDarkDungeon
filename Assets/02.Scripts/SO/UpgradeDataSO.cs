using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Item/UpgradeData")]
public class UpgradeDataSO : ScriptableObject
{
    [SerializeField] private List<UpgradeData> _grades;
    private  Dictionary<EItemGrade, UpgradeData> _gradeDict;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _gradeDict = new();
        foreach (var grade in _grades)
        {
            if (_gradeDict.TryAdd(grade.Grade, grade)) continue;
            Debug.LogWarning($"중복된 Grade 발견: {grade.Grade}");
        }
    }

    public UpgradeData? GetGradeInfo(EItemGrade grade)
    {
        if (_gradeDict.TryGetValue(grade, out UpgradeData data))
        {
            return data;
        }
        return null;
    }
}
