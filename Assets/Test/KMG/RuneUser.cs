using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;

    public ItemUpgradeDataSO UpgradeDB;
    public ItemDatabaseSO ItemDB;
    
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
}
