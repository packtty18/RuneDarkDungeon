using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>, IDataHandler
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;

    [SerializeField] private ItemDatabaseSO _itemDB;
    [SerializeField] private ItemUpgradeDataSO _upgradeDB;
    
    public ItemDatabaseSO ItemDB => _itemDB;
    public ItemUpgradeDataSO UpgradeDB => _upgradeDB;
    
    public ItemData ItemQ;
    public ItemData ItemE;
    public ItemData ItemR;

    public float QCooltime;
    public float ECooltime;
    public float RCooltime;

    public float NextQ;
    public float NextE;
    public float NextR;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > NextQ)
        {
            Debug.Log("Q 사용");
            ItemDB.UseItem(gameObject, ItemQ);
            NextQ = Time.time + QCooltime;
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > NextE)
        {
            Debug.Log("E 사용");
            ItemDB.UseItem(gameObject, ItemE);
            NextE = Time.time + ECooltime;
        }

        if (Input.GetKeyDown(KeyCode.R) && Time.time > NextR)
        {
            Debug.Log("R 사용");
            ItemDB.UseItem(gameObject, ItemR);
            NextR = Time.time + RCooltime;
        }
    }

    public void Save() { }
}
