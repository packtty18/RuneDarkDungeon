using UnityEngine;

public static class ItemGradeExtensions
{
    public static readonly EItemGrade MaxGrade = EItemGrade.Legendary;
    
    public static bool IsMaxGrade(this EItemGrade grade)
    {
        return grade == MaxGrade;
    }
    
    public static EItemGrade Next(this EItemGrade grade)
    {
        if (grade.IsMaxGrade()) return grade;
        return grade + 1;
    }
}
