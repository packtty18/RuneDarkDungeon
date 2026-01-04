using System.Collections.Generic;
using System;
using UnityEngine;

// 테스트용 룬 데이터
[System.Serializable]
public class RuneData
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }

    public RuneData(int id, string name, int level = 1)
    {
        ID = id;
        Name = name;
        Level = level;
    }
    
    public void LevelUp() => Level++;
    
    public override string ToString() => $"[ID:{ID}] {Name} (Lv.{Level})";
}

[System.Serializable]
public class GoldData : StatBase<int>
{
    [SerializeField] private int _amount;
    public int Amount => _amount;

    private void SetAmount(int amount)
    {
        int clamped = Math.Max(0, amount);

        if (_amount == clamped) return;

        _amount = clamped;
        Notify(_amount);
    }
    
    public void Add(int amount)
    {
        SetAmount(_amount + amount);
    }

    public bool TryConsume(int cost)
    {
        if (_amount < cost) return false;
        
        SetAmount(_amount - cost);
        return true;
    }
}

[System.Serializable]
public class GameData
{
    public GoldData Gold { get; private set; } = new();
    public List<RuneData> Runes { get; private set; } = new();
    
    public string GetSummary()
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine("==== Game Data Summary ====");
        sb.AppendLine($"Gold: {Gold.Amount}");
        sb.AppendLine($"Rune Count: {Runes.Count}");
        
        for (int i = 0; i < Runes.Count; i++)
        {
            sb.AppendLine($"- {i+1}. {Runes[i].ToString()}");
        }
        sb.AppendLine("===========================");
        return sb.ToString();
    }
}