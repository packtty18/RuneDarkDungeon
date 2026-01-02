using System.Collections.Generic;

// 테스트용 룬 데이터
[System.Serializable]
public class RuneData
{
    public int ID;
    public string Name;
    public int Level;

    public RuneData(int id, string name, int level = 1)
    {
        ID = id;
        Name = name;
        Level = level;
    }
    
    public override string ToString() => $"[ID:{ID}] {Name} (Lv.{Level})";
}

[System.Serializable]
public class GoldData
{
    public int Amount;
    
    public GoldData(int amount)
    {
        Amount = amount;
    }
}

[System.Serializable]
public class GameData
{
    public GoldData Gold = new(0);
    public List<RuneData> Runes = new();
    
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