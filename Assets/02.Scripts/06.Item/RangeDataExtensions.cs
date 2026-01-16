using UnityEngine;

public static class RangeDataExtensions
{
    public static int GetRandomValue(this in RangeData<int> range)
    {
        return Random.Range(range.Min, range.Max + 1); 
    }

    public static float GetRandomValue(this in RangeData<float> range)
    {
        return Random.Range(range.Min, range.Max);
    }

    public static EItemGrade GetRandomValue(this in RangeData<EItemGrade> range)
    {
        return (EItemGrade)Random.Range((int)range.Min, (int)range.Max + 1);
    }
}
