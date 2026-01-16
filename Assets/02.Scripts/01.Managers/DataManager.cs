using System.Collections.Generic;
using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>, IDataHolder
{
    private GameData _data = new();
    
    private Inventory _upgradeInventory = new();
    private Forge _forge;
    
    public IInventory Inventory => _data.Inventory;
    public IInventory UpgradeInventory => _upgradeInventory;
    public IEquipment Equipment => _data.Equipment;
    public ICurrency GoldData => _data.Gold;
    public IForge Forge => _forge;
    
    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);

        ItemFactory itemFactory = new(_itemDB);
        _forge = new(itemFactory, _upgradeDB, UpgradeInventory, Inventory, GoldData);
        
        itemFactory.SetItemInfo(Inventory.Items);
        itemFactory.SetItemInfo(Equipment.Items.Values);
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
