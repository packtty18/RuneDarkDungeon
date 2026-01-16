using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    private ItemDatabaseSO _itemDB;

    public ItemFactory(ItemDatabaseSO itemDB)
    {
        _itemDB = itemDB;
    }

    public void SetItemInfo(ItemData item)
    {
        ItemSO info = _itemDB.GetItemInfo(item);
        item.SetInfo(info);
    }
    
    public void SetItemInfo(IEnumerable<ItemData> items)
    {
        foreach (var item in items)
        {
            if (item == null) continue;
            SetItemInfo(item);
        }
    }
    
    public ItemData CreateUpgradedItem(ItemData baseItem)
    {
        ItemData newItem = baseItem.GetUpgradedItem();
        SetItemInfo(newItem);

        return newItem;
    }

    public ItemData CreateRandomItem(EItemGrade grade)
    {
        int id = Random.Range(0, _itemDB.Count);
        ItemData newItem = new ItemData(id, grade);
        SetItemInfo(newItem);
        return newItem;
    }
}
