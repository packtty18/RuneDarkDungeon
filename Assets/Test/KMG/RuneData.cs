using System.Collections.Generic;
using System;
using UnityEngine;

// 테스트용 룬 데이터
[System.Serializable]
public class RuneData
{
    [SerializeField] private int _id;
    [SerializeField] private int _level;

    public RuneData(int id, int level = 1)
    {
        _id = id; 
        _level = level;
    }
    
    public void LevelUp() => ++_level;
    
    public override string ToString() => $"[ID:{_id}] Lv.{_level})";
}

[System.Serializable]
public class GoldData : StatBase<int>, IReadOnlyValue<int>
{
    [SerializeField] private int _value;
    public int Value => _value;

    private void SetAmount(int amount)
    {
        int clamped = Math.Max(0, amount);

        if (_value == clamped) return;

        _value = clamped;
        Notify(_value);
    }
    
    public void Add(int amount)
    {
        SetAmount(_value + amount);
    }

    public bool TryConsume(int cost)
    {
        if (_value < cost) return false;
        
        SetAmount(_value - cost);
        return true;
    }
}

[System.Serializable]
public class GameData
{
    [SerializeField] private GoldData _gold;
    [SerializeField] private Inventory _inventory;

    public GoldData Gold => _gold;
    public Inventory Inventory => _inventory;
    
    public string GetSummary()
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine("==== Game Data Summary ====");
        sb.AppendLine($"Gold: {_gold.Value}");
        sb.AppendLine($"Rune Count: {_inventory.Runes.Count}");
        
        for (int i = 0; i < _inventory.Runes.Count; i++)
        {
            sb.AppendLine($"- {i+1}. {_inventory.Runes[i].ToString()}");
        }
        sb.AppendLine("===========================");
        return sb.ToString();
    }
}