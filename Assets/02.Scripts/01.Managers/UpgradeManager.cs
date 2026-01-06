using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private readonly Inventory _ingredients = new();
    public IReadOnlyInventory Ingredients => _ingredients;
    
    // Todo: 강화 정보 SO를 만들어서 가격과 재료 개수 캐싱
    private ItemData _targetType;
    private int _price;
    private int _count;
    
    // Todo: UI에게 강화 대상을 알려줄 event 추가
    
    // Todo: UI에게 강화 버튼 활성화 / 비활성화 여부를 알려주는 event 추가
    
    public bool TryRegister(ItemData item)
    {
        if (_targetType == null)
        {
            RegisterTargetType(item);
        }

        if (!item.TypeEquals(_targetType) || _ingredients.Items.Count >= _count) return false;
        
        InventoryManager.Instance.RemoveItem(item);
        _ingredients.Add(item);
        return true;
    }

    public ItemData Upgrade()
    {
        if (!GoldManager.Instance.UseGold(_price)) return null;
        
        _ingredients.Clear();
        ItemData newItem = new(_targetType.ID, _targetType.Grade + 1);
        InventoryManager.Instance.AddItem(newItem);

        return newItem;
    }

    public void Unregister(ItemData item)
    {
        InventoryManager.Instance.AddItem(item);
        _ingredients.Remove(item);

        if (_ingredients.Items.Count > 0) return;
        _targetType = null;
    }

    public void UnregisterAll()
    {
        foreach (var item in _ingredients.Items)
        {
            InventoryManager.Instance.AddItem(item);
        }
        _ingredients.Clear();
        _targetType = null;
    }

    // 테스트를 위해 임시로 구현
    private void RegisterTargetType(ItemData item)
    {
        _targetType = item;
        _price = 0;
        _count = 3;
    }
}
