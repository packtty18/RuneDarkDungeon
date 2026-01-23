using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnCoinCollected;
    public static event Action<EItemGrade> OnRuneCollected;

    public static void NotifyCoinCollected(int amount)
    {
        OnCoinCollected?.Invoke(amount);
    }
    
    public static void NotifyRuneCollected(EItemGrade grade)
    {
        OnRuneCollected?.Invoke(grade);
    }
}
