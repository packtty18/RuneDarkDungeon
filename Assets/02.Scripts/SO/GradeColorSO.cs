using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GradeColorData", menuName = "Item/GradeColorData")]
public class GradeColorSO : ScriptableObject
{
    [SerializeField] private List<GradeData> _colors;
    private Dictionary<EItemGrade, Color> _colorDict;

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        _colorDict = new();
        foreach (var color in _colors)
        {
            if (_colorDict.TryAdd(color.Grade, color.Color)) continue;
            Debug.LogWarning($"중복된 Grade 발견: {color.Grade}");
        }
    }

    public Color GetColor(EItemGrade grade)
    {
        return _colorDict.GetValueOrDefault(grade);
    }
}
