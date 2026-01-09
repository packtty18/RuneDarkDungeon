using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    public IInventory Inventory => _data.Inventory;
    public ICurrency GoldData => _data.Gold;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    [SerializeField] private GradeColorSO _colorDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;
    public GradeColorSO ColorDB => _colorDB;
    
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
