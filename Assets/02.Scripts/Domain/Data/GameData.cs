using UnityEngine;

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
        sb.AppendLine($"Rune Count: {_inventory.Items.Count}");
        
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            sb.AppendLine($"- {i+1}. {_inventory.Items[i].ToString()}");
        }
        sb.AppendLine("===========================");
        return sb.ToString();
    }
}