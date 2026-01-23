using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>, IDataHolder
{
    private GameData _data = new();
    
    private Forge _forge;
    
    public IInventory Inventory => _data.Inventory;
    public IEquipment Equipment => _data.Equipment;
    public ICurrency GoldData => _data.Gold;
    public IForge Forge => _forge;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    [SerializeField] private StartDataSO _startData;
    
    protected override void OnInit()
    {
        if (FileIO.Load(_data))
        {
            InitializeItems();
        }
        else
        {
            CreateNewGameData();
        }
    }

    private void InitializeItems()
    {
        ItemFactory itemFactory = new(_itemDB);
        _forge = new(itemFactory, _upgradeDB);
        
        itemFactory.SetItemInfo(Inventory.Items);
        itemFactory.SetItemInfo(Equipment.Items.Values);
    }

    public void CreateNewGameData()
    {
        GoldData gold = new(_startData.Gold);
        Inventory inventory = new(_startData.Inventory);
        Equipment equipment = new(_startData.Equipment);
        
        _data = new(gold, inventory, equipment);

        InitializeItems();
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
