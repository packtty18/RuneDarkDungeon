using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneUser : MonoBehaviour
{
    private void Start()
    {
        Test();
    }
    
    private void Test()
    {
        IReadOnlyList<ItemData> items = InventoryManager.Instance.Inventory.Items;

        foreach (var item in items)
        {
            ItemSO itemInfo = InventoryManager.Instance.GetItemInfo(item.ID);
            Debug.Log($"{item.Grade} {itemInfo.Name} 사용");
            itemInfo.Use(gameObject, item.Grade);
        }
    }
}
