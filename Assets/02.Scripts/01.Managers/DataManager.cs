using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>, IDataHandler
{
    private GameData _data = new();
    
    private UpgradeInventory _upgradeInventory = new();
    
    public IInventory Inventory => _data.Inventory;
    public IInventory UpgradeInventory => _upgradeInventory;
    public IEquipment Equipment => _data.Equipment;
    public ICurrency GoldData => _data.Gold;
    
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;
    
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
