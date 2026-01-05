using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    protected override void OnInit()
    {
        FileIO.Load(_data);
    }

    private void Start()
    {
        GoldManager.Instance.Initialize(_data.Gold);
        InventoryManager.Instance.Initialize(_data.Inventory);
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
