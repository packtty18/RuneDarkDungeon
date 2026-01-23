using UnityEngine;

public class Item_Rune : ItemBase
{
    [Header("룬 등급")]
    [SerializeField] private SerializableDictionary<EItemGrade, Color> _colorDict;
    [SerializeField] private Light _light;
    [SerializeField] private RangeData<EItemGrade> _gradeRange;
    private EItemGrade _grade;
    
    private void OnEnable()
    {
        DetermineRandomGrade();
    }

    private void DetermineRandomGrade()
    {
        _grade = _gradeRange.GetRandomValue();
        _light.color = _colorDict[_grade];
    }
    
    protected override void OnCollected()
    {
        GameEvents.NotifyRuneCollected(_grade);
    }
}
