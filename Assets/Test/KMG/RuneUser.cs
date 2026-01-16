using System.Collections.Generic;
using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>, IDataHolder
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    [SerializeField] private Equipment _equipment;
    private Forge _forge;
    
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;
    public IEquipment Equipment => _equipment;
    public IForge Forge => _forge;

    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    [Header("연결 대상 플레이어")]
    [SerializeField] private PlayerSkillCaster _playerSkillCaster;
    
    protected override void OnInit()
    {
        ItemFactory itemFactory = new(_itemDB);
        _forge = new(itemFactory, _upgradeDB);
        
        itemFactory.SetItemInfo(Inventory.Items);
        itemFactory.SetItemInfo(Equipment.Items.Values);
        
        _playerSkillCaster.Initialize(_equipment);
    }

    public void Save() { }
}
