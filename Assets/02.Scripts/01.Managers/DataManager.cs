using UnityEngine;

[RequireComponent(typeof(GoldManager), typeof(InventoryManager))]
public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    protected override void OnInit()
    {
        FileIO.Load(_data);
        GetComponent<GoldManager>().Initialize(_data.Gold);
        GetComponent<InventoryManager>().Initialize(_data.Inventory);
    }

    public void Save()
    {
        FileIO.Save(_data);
    }
    
    private void OnApplicationQuit()
    {
        FileIO.Save(_data);
    }
}
