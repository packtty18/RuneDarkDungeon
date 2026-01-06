using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    public Inventory Inventory => _data.Inventory;
    public GoldData GoldData => _data.Gold;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private UpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public UpgradeDataSO UpgradeDB => _upgradeDB;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);
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
