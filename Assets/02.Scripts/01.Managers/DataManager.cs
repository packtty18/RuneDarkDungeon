using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    public IInventory Inventory => _data.Inventory;
    public ICurrency GoldData => _data.Gold;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);
        TestAdd();
    }

    private void TestAdd()
    {
        GoldData.Add(10000);

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
