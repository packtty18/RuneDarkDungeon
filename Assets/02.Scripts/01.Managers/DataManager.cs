using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>, IDataHandler
{
    private GameData _data = new();
    
    private UpgradeInventory _upgradeInventory;
    
    public IInventory Inventory => _data.Inventory;
    public IUpgradeInventory UpgradeInventory => _upgradeInventory;
    public ICurrency GoldData => _data.Gold;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);
        _upgradeInventory = new(_upgradeDB);
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
