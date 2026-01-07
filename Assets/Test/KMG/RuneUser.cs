using System.Collections.Generic;
using UnityEngine;

public class RuneUser : LocalSingleton<RuneUser>
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private GoldData _goldData;
    public IInventory Inventory => _inventory;
    public ICurrency GoldData => _goldData;

    public UpgradeDataSO UpgradeDB;
    public ItemDatabaseSO ItemDB;
    public GradeColorSO ColorDB;
    
    private void Start()
    {
        Test();
    }
    
    private void Test()
    {
    }
}
