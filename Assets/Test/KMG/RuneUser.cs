using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>, IDataHandler
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    [SerializeField] private Equipment _equipment;
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;
    public IEquipment Equipment => _equipment;
    
    private UpgradeInventory _upgradeInventory = new();
    public IInventory UpgradeInventory => _upgradeInventory;

    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;

    public EquipmentManager equipmentManager;

    private void Start()
    {
        equipmentManager = new(_equipment, _inventory, _itemDB);
    }

    public float NextQ;
    public float NextE;
    public float NextR;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > NextQ)
        {
            Debug.Log("Q 사용");
            equipmentManager.UseItem(gameObject, ESkillSlot.Q, out var qCooltime);
            NextQ = Time.time + qCooltime;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > NextE)
        {
            Debug.Log("E 사용");
            equipmentManager.UseItem(gameObject, ESkillSlot.E, out var eCooltime);
            NextE = Time.time + eCooltime;
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time > NextR)
        {
            Debug.Log("R 사용");
            equipmentManager.UseItem(gameObject, ESkillSlot.R, out var rCooltime);
            NextR = Time.time + rCooltime;
        }
    }

    public void Save() { }
}
