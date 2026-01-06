using UnityEngine;

public class ItemUpgrader
{
    private readonly Inventory _upgradeInventory = new();
    public IReadOnlyInventory UpgradeInventory => _upgradeInventory;

    private UpgradeDataSO _upgradeDB;
    
    private ItemData _targetType;
    private int _price;
    private int _count;
    
    // Todo: UI에게 강화 대상을 알려줄 event 추가
    
    // Todo: UI에게 강화 버튼 활성화 / 비활성화 여부를 알려주는 event 추가

    public ItemUpgrader(UpgradeDataSO upgradeDB)
    {
        _upgradeDB = upgradeDB;
    }
    
    public bool TryRegister(ItemData item)
    {
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }

        if (!item.TypeEquals(_targetType) || _upgradeInventory.Count >= _count) return false;
        
        InventoryManager.Instance.RemoveItem(item);
        _upgradeInventory.Add(item);
        return true;
    }

    public ItemData Upgrade()
    {
        if (!GoldManager.Instance.UseGold(_price)) return null;
        
        _upgradeInventory.Clear();
        ItemData newItem = new(_targetType.ID, _targetType.Grade + 1);
        InventoryManager.Instance.AddItem(newItem);

        return newItem;
    }

    public void Unregister(ItemData item)
    {
        InventoryManager.Instance.AddItem(item);
        _upgradeInventory.Remove(item);

        if (_upgradeInventory.Items.Count > 0) return;
        _targetType = null;
    }

    public void UnregisterAll()
    {
        foreach (var item in _upgradeInventory.Items)
        {
            InventoryManager.Instance.AddItem(item);
        }
        _upgradeInventory.Clear();
        _targetType = null;
    }

    private void RegisterTargetType(ItemData item)
    {
        _targetType = item;
        
        var info = _upgradeDB.GetGradeInfo(item.Grade);
        if (info == null) return;
        
        _price = info.Value.UpgradePrice;
        _count = info.Value.IngredientCount;
    }
}
