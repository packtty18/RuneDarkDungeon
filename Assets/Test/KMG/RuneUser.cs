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
    private Inventory _upgradeInventory = new();
    public IInventory UpgradeInventory => _upgradeInventory;

    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public float NextQ;
    public float NextE;
    public float NextR;
    
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    protected override void OnInit()
    {
        ItemFactory itemFactory = new(_itemDB);
        _forge = new(itemFactory, _upgradeDB, _upgradeInventory, _inventory, _goldData);
        
        itemFactory.SetItemInfo(Inventory.Items);
        itemFactory.SetItemInfo(Equipment.Items.Values);
        
        _initializer.Initialize(this);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > NextQ)
        {
            Debug.Log("Q 사용");
            var item = _equipment.GetItem(ESkillSlot.Q);
            item.Use(gameObject);
            NextQ = Time.time + item.GetCoolTime();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > NextE)
        {
            Debug.Log("E 사용");
            var item = _equipment.GetItem(ESkillSlot.E);
            item.Use(gameObject);
            NextQ = Time.time + item.GetCoolTime();
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time > NextR)
        {
            Debug.Log("R 사용");
            var item = _equipment.GetItem(ESkillSlot.R);
            item.Use(gameObject);
            NextQ = Time.time + item.GetCoolTime();
        }
    }

    public void Save() { }
}
