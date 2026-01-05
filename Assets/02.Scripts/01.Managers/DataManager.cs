using UnityEngine;

public class DataManager : GlobalSingleton<DataManager>
{
    private GameData _data = new();

    [SerializeField] private ItemDatabaseSO _itemDB;
    
    protected override void OnInit()
    {
        FileIO.Load(_data);
        _itemDB.Initialize();
        
        gameObject.AddComponent<GoldManager>();
        gameObject.AddComponent<InventoryManager>();
        
        GoldManager.Instance.Initialize(_data.Gold);
        InventoryManager.Instance.Initialize(_data.Inventory);
    }

    public ItemSO GetItemInfo(int id)
    {
        return _itemDB.GetItem(id);
    }
    
    private void AddData()
    {
        GoldManager.Instance.AddGold(1000);
        InventoryManager.Instance.AddItem(new ItemData(0));
        InventoryManager.Instance.AddItem(new ItemData(1, 5));
    }
    
    private void OnApplicationQuit()
    {
        //AddData();
        FileIO.Save(_data);
    }
}
