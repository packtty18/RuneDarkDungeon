using System.Collections.Generic;
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
        if (!FileIO.Load(_data))
        {
            CreateNewGameData();
        }

        ItemFactory itemFactory = new(_itemDB);
        _forge = new(itemFactory, _upgradeDB);
        
        itemFactory.SetItemInfo(Inventory.Items);
        itemFactory.SetItemInfo(Equipment.Items.Values);
    }

    public void CreateNewGameData()
    {
        GoldData gold = new();
        Inventory inventory = new();
        Equipment equipment = new();
        
        gold.Add(_startData.Gold);
        
        foreach (var item in _startData.Inventory)
        {
            inventory.Add(item);
        }

        foreach (var item in _startData.Equipment)
        {
            equipment.Equip(item.Key, item.Value);
        }
        
        _data = new(gold, inventory, equipment);
        
        ItemFactory itemFactory = new(_itemDB);
        
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
