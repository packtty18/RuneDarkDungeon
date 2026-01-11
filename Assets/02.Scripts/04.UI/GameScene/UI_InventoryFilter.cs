using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryFilter
{
    public void ResetFilter(IReadOnlyList<UI_Slot> slots)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            slot.SetFilter(true);
        }
    }
    
    public void RefreshFilter(IInventory inventory, IReadOnlyList<UI_Slot> slots)
    {
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.SetActive(false);
                continue;
            }
            bool isOn = inventory.CanAdd(slot.Item);
            slot.SetFilter(isOn);
            slot.SetActive(true);
        }
    }
}
