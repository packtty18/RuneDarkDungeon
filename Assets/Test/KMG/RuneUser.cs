using System.Collections.Generic;
using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>, IDataHandler
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
    
    public ItemDatabaseSO ItemDB => _itemDB;
    
    public float NextQ;
    public float NextE;
    public float NextR;
    
    [Header("연결 대상 UI")]
    [SerializeField] private UI_Initializer _initializer;

    protected override void OnInit()
    {
        _forge = new(_upgradeDB, _upgradeInventory, _inventory, _goldData);
        
        SetItemInfo(Inventory.Items);
        SetItemInfo(Equipment.Items.Values);
        
        _initializer.Initialize(this);
    }
    
    private void SetItemInfo(IEnumerable<ItemData> items)
    {
        foreach (var item in items)
        {
            if (item == null) continue;
            ItemSO info = _itemDB.GetItemInfo(item);
            item.SetInfo(info);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > NextQ)
        {
            Debug.Log("Q 사용");
            _equipment.UseItem(gameObject, ESkillSlot.Q, out var qCooltime);
            NextQ = Time.time + qCooltime;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > NextE)
        {
            Debug.Log("E 사용");
            _equipment.UseItem(gameObject, ESkillSlot.E, out var eCooltime);
            NextE = Time.time + eCooltime;
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time > NextR)
        {
            Debug.Log("R 사용");
            _equipment.UseItem(gameObject, ESkillSlot.R, out var rCooltime);
            NextR = Time.time + rCooltime;
        }
    }

    public void Save() { }
}
