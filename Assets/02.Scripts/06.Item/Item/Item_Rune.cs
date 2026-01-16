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
        int min = (int)_gradeRange.Min;
        int max = (int)_gradeRange.Max + 1;
        
        int randomValue = Random.Range(min, max);
        _grade = (EItemGrade)randomValue;
        Debug.Log(_grade);
        _light.color = _colorDict[_grade];
    }
    
    protected override void OnCollected()
    {
        GameEvents.NotifyRuneCollected(_grade);
        Debug.Log("룬 획득");
    }
}
