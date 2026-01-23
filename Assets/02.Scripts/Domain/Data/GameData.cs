using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private GoldData _gold;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Equipment _equipment;
    
    public GoldData Gold => _gold;
    public Inventory Inventory => _inventory;
    public Equipment Equipment => _equipment;

    public GameData(GoldData gold = null, Inventory inventory = null, Equipment equipment = null)
    {
        _gold = gold ?? new();
        _inventory = inventory ?? new();
        _equipment = equipment ?? new();
    }
    
    public string GetSummary()
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine("==== Game Data Summary ====");
        sb.AppendLine($"Gold: {_gold.Amount}");
        sb.AppendLine($"Rune Count: {_inventory.Items.Count}");
        
        for (int i = 0; i < _inventory.Items.Count; i++)
        {
            sb.AppendLine($"- {i+1}. {_inventory.Items[i].ToString()}");
        }
        sb.AppendLine("===========================");
        return sb.ToString();
    }
}